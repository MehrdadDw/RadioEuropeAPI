using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using RadioEurope.API.Services;
using RadioEurope.API.Binders;
using RadioEurope.API.Models;
namespace RadioEurope.API.Controllers.v1;
[ApiController]
[Route("v1/diff/")]
public class DiffController : ControllerBase
{
    private readonly ILogger<DiffController> _logger;
    private readonly IRedisService _redisService;
    private readonly IDiffService _diffService;

    public DiffController(ILogger<DiffController> logger,IRedisService redisService,IDiffService diffService)
    {
        //var lazyConnection = ConnectionMultiplexer.Connect("localhost");
        _redisService = redisService;//new RedisService(lazyConnection);
        _diffService=diffService;//new DiffService(null);
        _logger = logger;
    }

    [Route("{ID}/left")]
    [HttpPost]
    [Consumes("application/custom")]

    public async Task<IActionResult> left(string ID,[FromBody][ModelBinder(typeof(CustomBinder))]  string input="\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"")
    {
        _redisService.Write(new LeftRightDiff { Left = input, Right = " ", ID = ID });
        return Ok();
    } 

    [Route("{ID}/right")]
    [HttpPost]
    [Consumes("application/custom")]
    public async Task<IActionResult> right(string ID,[FromBody][ModelBinder(typeof(CustomBinder))]  string input="\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"")
    {
        _redisService.Write(new LeftRightDiff { Left = " ", Right = input, ID = ID });
        return Ok();
    }
    [Route("{ID}")]
    [HttpGet]
    public async Task<IActionResult> diff(string ID)
    {
        var res = _diffService.CalculateDiff(ID) ;
        return Ok(res);
    }
}
