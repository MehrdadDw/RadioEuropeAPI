using RadioEurope.API.Models;
using RadioEurope.API.Models.Enums;
using RadioEurope.API.Application.Interfaces;
namespace RadioEurope.API.Application.Services;


public class DiffService : IDiffService
{
    private readonly IDataService _DataService;

    public DiffService(IDataService DataService)
    {
        _DataService = DataService;

    }
    public async Task<LeftRightDiff?> RetrieveLRD(string ID)
    {
        return await _DataService.ReadLRD(ID);
    }
    public async Task<CalculateResult> CalculateDiff(string ID)
    {
        var result = new List<OffsetLength>();
        var LRD=await RetrieveLRD(ID);
        //todo not found
        if (LRD==null){
        return new CalculateResult{ Message=DiffMessage.KeyNotFound,Data= result};
        }
        if (LRD.Left==LRD.Right){
            return new CalculateResult{ Message= DiffMessage.Equal,Data= result};
        }
        if (LRD.Left==null || LRD.Right==null || LRD.Left.Length!=LRD.Right.Length){
            return new CalculateResult{ Message= DiffMessage.LengthsNotEqual,Data= result};
        }
        var offset = 0;
        var lenght = 0;
        for (int i = 0; i < LRD.Left.Length; i++)
        {
            if (LRD.Left[i] != LRD.Right[i])
            {
                if (lenght == 0)
                {
                    offset = i;
                }
                lenght++;
            }
            else
            {
                if (lenght != 0)
                {
                    result.Add(new OffsetLength { offset = offset, length = lenght });
                    lenght = 0;
                }
            }

        }
            if (lenght != 0)
            {
                result.Add(new OffsetLength { offset = offset, length = lenght });
            }
        return new CalculateResult{ Message= DiffMessage.Success,Data= result};
    }

}