using Microsoft.AspNetCore.Mvc;
using RadioEurope.API.Services;
using RadioEurope.API.Binders;
using RadioEurope.API.Models;
using RadioEurope.API.Models.Enums;
using Swashbuckle.AspNetCore.Annotations;
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
    [SwaggerOperation(Summary = "Receives the value of Left element by ID.")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/custom")]
    public async Task<IActionResult> left(
    [SwaggerParameter("The ID of the element", Required = true)] string ID,
    [SwaggerParameter("The Base64 encoded Left value of element.", Required = true)]
    [FromBody][ModelBinder(typeof(CustomBinder))] string input = "\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"")
    {
        try
        {
            await _DataService.Write(new LeftRightDiff { Left = input, Right = " ", ID = ID });
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during {nameof(left)} call", ex);
            throw;
        }
    }

    [Route("{ID}/right")]
    [SwaggerOperation(Summary = "Receives the value of Right element by ID.")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("application/custom")]
    public async Task<IActionResult> right(
    [SwaggerParameter("The ID of the element", Required = true)] string ID,
     [SwaggerParameter("The Base64 encoded Right value of element.", Required = true)]
      [FromBody][ModelBinder(typeof(CustomBinder))] string input = "\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"")
    {
        try
        {
            await _DataService.Write(new LeftRightDiff { Left = " ", Right = input, ID = ID });
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during {nameof(right)} call", ex);
            throw;
        }
    }

    [Route("{ID}")]
    [SwaggerOperation(Summary = "Differentiates the value of Right and Left elements by ID and returns the result.")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<OffsetLength>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpGet]
    public async Task<IActionResult> diff(
        [SwaggerParameter("The ID of the element", Required = true)] string ID)
    {
        try
        {
            var res = await _diffService.CalculateDiff(ID);
            switch (res.Message)
            {
                case DiffMessage.Equal:
                    return Ok(new { message = "Inputs were equal.", data = res.Data });
                case DiffMessage.KeyNotFound:
                    return NotFound(new { message = "Couldn't find the ID.", data = res.Data });
                case DiffMessage.LengthsNotEqual:
                    return Ok(new { message = "Inputs are of different size.", data = res.Data });
                case DiffMessage.Success:
                    return Ok(new { message = "Success", data = res.Data });
                default:
                    return Ok(new { message = "", data = res.Data });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error during {nameof(diff)} call", ex);
            throw;
        }

    }
}
