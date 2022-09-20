using RadioEurope.API.Models;
namespace RadioEurope.API.Application.Interfaces;
public interface IDiffService
{
    Task<LeftRightDiff?> RetrieveLRD(string ID);
    Task<CalculateResult> CalculateDiff(string ID);
}