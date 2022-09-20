using RadioEurope.API.Models;
namespace RadioEurope.API.Application.Interfaces;
public interface IDataService
{
    /// <summary>
    /// Method <c>Write</c> Writes or overwrites a LeftRightDiff object to the data store(redis).
    /// </summary>
    Task Write(LeftRightDiff LRD);
    /// <summary>
    /// Method <c>Write</c> Reads a LeftRightDiff from data store(redis) by ID.
    /// </summary>
    Task<LeftRightDiff?> ReadLRD(string ID);
}