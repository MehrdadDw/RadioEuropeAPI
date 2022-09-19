using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using RadioEurope.API.Services;
using RadioEurope.API.Binders;
using RadioEurope.API.Models;
using RadioEurope.API.Models.Enums;
namespace RadioEurope.API.Controllers.v1;
[ApiController]
[Route("v1/diff/")]
public class DiffController : ControllerBase
{
    private readonly ILogger<DiffController> _logger;
    private readonly IDataService _DataService;
    private readonly IDiffService _diffService;

    public DiffController(ILogger<DiffController> logger, IDataService DataService, IDiffService diffService)
    {
        _DataService = DataService;
        _diffService = diffService;
        _logger = logger;
    }

    [Route("{ID}/left")]
    [HttpPost]
    [Consumes("application/custom")]
    public async Task<IActionResult> left(string ID, [FromBody][ModelBinder(typeof(CustomBinder))] string input = "\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"")
    {
        try
        {
            _DataService.Write(new LeftRightDiff { Left = input, Right = " ", ID = ID });
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during {nameof(left)} call",ex);
            throw ex;
        }
    }

    [Route("{ID}/right")]
    [HttpPost]
    [Consumes("application/custom")]
    public async Task<IActionResult> right(string ID, [FromBody][ModelBinder(typeof(CustomBinder))] string input = "\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"")
    {
        try
        {
            _DataService.Write(new LeftRightDiff { Left = " ", Right = input, ID = ID });
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during {nameof(right)} call",ex);
            throw ex;
        }
    }

    [Route("{ID}")]
    [HttpGet]
    public async Task<IActionResult> diff(string ID)
    {
        try
        {
            var res = _diffService.CalculateDiff(ID);
            switch (res.Message)
            {
                case DiffMessage.Equal:
                    return Ok(new { message = "Inputs were equal.", data = res.Data });
                    break;
                case DiffMessage.KeyNotFound:
                    return NotFound(new { message = "Couldn't find the ID.", data = res.Data });
                    break;
                case DiffMessage.LengthsNotEqual:
                    return Ok(new { message = "Inputs are of different size.", data = res.Data });
                    break;
                case DiffMessage.Success:
                    return Ok(new { message = "Success", data = res.Data });
                    break;
                default:
                    return Ok(new { message = "", data = res.Data });
                    break;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during {nameof(diff)} call",ex);
            throw ex;
        }

    }
}
