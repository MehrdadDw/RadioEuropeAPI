using StackExchange.Redis;
using RadioEurope.API.Models;
namespace RadioEurope.API.Services;
public interface IDiffService

{
    LeftRightDiff RetrieveLRD(string ID);
    List<OffsetLength> CalculateDiff(string ID);
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
    public List<OffsetLength> CalculateDiff(string ID)
    {
        var LRD=RetrieveLRD(ID);
        var result = new List<OffsetLength>();
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
        return result;
    }

}