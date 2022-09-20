using RadioEurope.API.Models.Enums;
namespace RadioEurope.API.Models;
/// <summary>
/// Class <c>CalculateResult</c> models a list of differences in a wraped response.
/// </summary>
public class CalculateResult{
    public DiffMessage Message { get; set; }
    public List<OffsetLength>? Data { get; set; }
}