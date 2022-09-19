using Moq;
using RadioEurope.API.Services;
using RadioEurope.API.Models;
using StackExchange.Redis;
using RadioEurope.API.Controllers.v1;
using Microsoft.AspNetCore.Mvc;
namespace RadioEurope.UnitTests;

public class TestDiffController
{
    [Fact]
    public async Task Left_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(new LeftRightDiff{Left="l",Right="r",ID="id1"});

        var sut=new DiffController(null,mockDataService.Object,null);
        //Act
        var result=(OkResult)sut.left("id1").Result;
        //Assert
		Assert.Equal(result.StatusCode,200);
    }
    [Fact]
    public async Task Right_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(new LeftRightDiff{Left="l",Right="r",ID="id1"});

        var sut=new DiffController(null,mockDataService.Object,null);
        //Act
        var result=(OkResult)sut.right("id1").Result;
        //Assert
		Assert.Equal(result.StatusCode,200);
    }
    [Fact]
    public async Task diff_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(new LeftRightDiff{Left="l",Right="r",ID="id1"});
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase=true;
        var sut=new DiffController(null,mockDataService.Object,mockDiffService.Object);
        //Act
        var result=(OkObjectResult)(await sut.diff("id1"));
        //Assert
		Assert.Equal(result.StatusCode,200);
    }
    [Fact]
    public async Task Right_OnSuccess_InvokesRedisRevice()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        var sut=new DiffController(null,mockDataService.Object,null);
        //Act
        var result=sut.right("id1");
        //Assert
        mockDataService.Verify(service=>service.Write(It.IsAny<LeftRightDiff>()),Times.Once());
    }
    [Fact]
    public async Task diff_OnSuccess_InvokesRedisRevice()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(new LeftRightDiff{Left="l",Right="r",ID="id1"});
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase=true;
        var sut=new DiffController(null,mockDataService.Object,mockDiffService.Object);
        //Act
        var result=(OkObjectResult)(await sut.diff("id1"));
        //Assert
		mockDataService.Verify(service=>service.ReadLRD(It.IsAny<string>()),Times.Once());
    }
     [Fact]
    public async Task diff_OnSuccess_InvokesDiffRevice()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(new LeftRightDiff{Left="l",Right="r",ID="id1"});
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase=true;
        var sut=new DiffController(null,mockDataService.Object,mockDiffService.Object);
        //Act
        var result=(OkObjectResult)(await sut.diff("id1"));
        //Assert
		mockDiffService.Verify(service=>service.CalculateDiff(It.IsAny<string>()),Times.Once());
    }
}