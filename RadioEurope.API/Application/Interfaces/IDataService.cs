using RadioEurope.API.Models;
namespace RadioEurope.API.Application.Interfaces;
public interface IDataService
{
    Task Write(LeftRightDiff LRD);
    Task<LeftRightDiff?> ReadLRD(string ID);
}