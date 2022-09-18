using RadioEurope.API.Models.Enums;
namespace RadioEurope.API.Models;

public class CalculateResult{
    public DiffMessage Message { get; set; }
    public List<OffsetLength>? Data { get; set; }
}