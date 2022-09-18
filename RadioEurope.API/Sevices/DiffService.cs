using StackExchange.Redis;
using RadioEurope.API.Models;
using RadioEurope.API.Models.Enums;
namespace RadioEurope.API.Services;
public interface IDiffService

{
    LeftRightDiff RetrieveLRD(string ID);
    CalculateResult CalculateDiff(string ID);
}

public class DiffService : IDiffService
{
    private readonly IRedisService _redisService;

    public DiffService(IRedisService redisService)
    {
        _redisService = redisService;

    }
    public LeftRightDiff RetrieveLRD(string ID)
    {
        return _redisService.ReadLRD(ID);
    }
    public CalculateResult CalculateDiff(string ID)
    {
        var result = new List<OffsetLength>();
        var LRD=RetrieveLRD(ID);
        //todo not found
        if (LRD==null){
        return new CalculateResult{ Message=DiffMessage.KeyNotFound,Data= result};
        }
        if (LRD.Left==LRD.Right){
            return new CalculateResult{ Message= DiffMessage.Equal,Data= result};
        }
        if (LRD.Left.Length!=LRD.Right.Length){
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