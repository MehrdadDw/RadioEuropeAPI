using StackExchange.Redis;
using RadioEurope.API.Models;
namespace RadioEurope.API.Services;
public interface IRedisService

{
    void Write(LeftRightDiff LRD);
    LeftRightDiff? ReadLRD(string ID);
}

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer? multiplexer;

    public RedisService(IConnectionMultiplexer multiplexer)
    {
        this.multiplexer = multiplexer;
    }

    public void Write(LeftRightDiff LRD)
    {
        if (multiplexer == null)
        {
            return ;
        }
        var database = multiplexer.GetDatabase(1);
        var LRD1 = ReadLRD(LRD.ID);
        var left = " ";
        var right = " ";
        if (LRD.Left != " ")
        {
            left = LRD.Left;
        }
        else if (!string.IsNullOrEmpty(LRD1?.Left))
        {
            left = LRD1.Left;
        }

        if (LRD.Right != " ")
        {
            right = LRD.Right;
        }
        else if (!string.IsNullOrEmpty(LRD1?.Right))
        {
            right = LRD1.Right;
        }
        database.HashSet(LRD.ID, new HashEntry[] { new HashEntry("Left", left), new HashEntry("Right", right) });     
    }
    public LeftRightDiff? ReadLRD(string ID)
    {
        var database = multiplexer.GetDatabase(1);
        if ( database.KeyExists(ID)){
        var retreivedLeft = database.HashGetAll(ID)[0].Value;
        var retreivedRight = database.HashGetAll(ID)[1].Value;
        return new LeftRightDiff { ID = ID,
          Left = retreivedLeft
        ,Right = retreivedRight };
        }
        return null;
    }
}