using RadioEurope.API.Models;
namespace RadioEurope.API.Application.Interfaces;
public interface IDiffService
{
    /// <summary>
    /// Method <c>RetrieveLRD</c> Reads a LeftRightDiff by ID with the help of underlying data service.
    /// </summary>
    Task<LeftRightDiff?> RetrieveLRD(string ID);
    /// <summary>
    /// Method <c>CalculateDiff</c> Calculates the differences of left and right by ID.
    /// </summary>
    Task<CalculateResult> CalculateDiff(string ID);
}