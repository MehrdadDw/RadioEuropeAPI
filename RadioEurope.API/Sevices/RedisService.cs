using StackExchange.Redis;
using RadioEurope.API.Models;
namespace RadioEurope.API.Services;
public interface IDataService

{
    void Write(LeftRightDiff LRD);
    LeftRightDiff? ReadLRD(string ID);
}

public class DataService : IDataService
{
    private readonly IConnectionMultiplexer? multiplexer;

    public DataService(IConnectionMultiplexer multiplexer)
    {
        this.multiplexer = multiplexer;
    }

    public void Write(LeftRightDiff Lrd)
    {
        if (multiplexer == null)
        {
            return ;
        }
        var database = multiplexer.GetDatabase(1);
        var lrd1 = ReadLRD(Lrd.ID);
        var left = " ";
        var right = " ";
        if (Lrd.Left != " ")
        {
            left = Lrd.Left;
        }
        else if (!string.IsNullOrEmpty(lrd1?.Left))
        {
            left = lrd1.Left;
        }

        if (Lrd.Right != " ")
        {
            right = Lrd.Right;
        }
        else if (!string.IsNullOrEmpty(lrd1?.Right))
        {
            right = lrd1.Right;
        }
        database.HashSet(Lrd.ID, new HashEntry[] { new HashEntry("Left", left), new HashEntry("Right", right) });     
    }
    public LeftRightDiff? ReadLRD(string Id)
    {
        var database = multiplexer.GetDatabase(1);
        if ( database.KeyExists(Id)){
        var retreivedLeft = database.HashGetAll(Id)[0].Value;
        var retreivedRight = database.HashGetAll(Id)[1].Value;
        return new LeftRightDiff { ID = Id,
          Left = retreivedLeft
        ,Right = retreivedRight };
        }
        return null;
    }
}