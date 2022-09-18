using Moq;
using RadioEurope.API.Services;
using RadioEurope.API.Models;
using StackExchange.Redis;
namespace RadioEurope.UnitTests;

public class Services
{
    [Theory]
    [InlineData("123456", "ab34ef",new object[] {new int[] {0,2},new int[]{4,2} })]
    [InlineData("123456", "abcdef",new object[] {new int[] {0,6} })]
    [InlineData("abc456", "abcdef",new object[] {new int[] {3,3} })]
    [InlineData("abcefghij", "0b0e0g0i0",new object[] {new int[] {0,1},new int[] {2,1}, new int[] {4,1}, new int[] {6,1}, new int[] {8,1}})]
    public void DiffService_Should_Calculate(string l,string r,object[] answer)
    {

        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockRedisService = new Mock<RedisService>(multiplexer).As<IRedisService>();
        mockRedisService.CallBase = true;
        mockRedisService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(new LeftRightDiff { Left = l, Right = r, ID = "id1" });
        var mockDiffService = new Mock<DiffService>(mockRedisService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var calculated = mockDiffService.Object.RetrieveLRD("id1");
        var DiffResult = mockDiffService.Object.CalculateDiff("id1");
        //Assert
		foreach (var item in answer)
		{
			Assert.True(DiffResult.Contains(new OffsetLength { offset = ((int[])item)[0],length=  ((int[])item)[1] }));
		}
		Assert.Equal(answer.Length,DiffResult.Count);
    }
}
