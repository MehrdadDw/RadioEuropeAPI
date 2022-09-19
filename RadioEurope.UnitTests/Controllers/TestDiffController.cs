using Moq;
using RadioEurope.API.Services;
using RadioEurope.API.Models;
using StackExchange.Redis;
using RadioEurope.API.Controllers.v1;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace RadioEurope.UnitTests;

public class TestDiffController
{
    ILogger<DiffController> logger=null;
    IDiffService DiffController=null;
    IConnectionMultiplexer multiplexer = null;
    [Fact]
    public async Task Left_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns( Task.FromResult<LeftRightDiff?>(new LeftRightDiff{Left="l",Right="r",ID="id1"}));

        var sut=new DiffController(logger,mockDataService.Object,DiffController);
        //Act
        var result=(OkResult)(await sut.left("id1"));
        //Assert
		Assert.Equal(200,result.StatusCode);
    }
    [Fact]
    public async Task Right_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns( Task.FromResult<LeftRightDiff?>(new LeftRightDiff{Left="l",Right="r",ID="id1"}));

        var sut=new DiffController(logger,mockDataService.Object,DiffController);
        //Act
        var result=(OkResult)(await sut.right("id1"));
        //Assert
		Assert.Equal(200,result.StatusCode);
    }
    [Fact]
    public async Task diff_OnSuccess_ReturnsStatusCode200()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns( Task.FromResult<LeftRightDiff?>(new LeftRightDiff{Left="l",Right="r",ID="id1"}));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase=true;
        var sut=new DiffController(logger,mockDataService.Object,mockDiffService.Object);
        //Act
        var result=(OkObjectResult)(await sut.diff("id1"));
        //Assert
		Assert.Equal(200,result.StatusCode);
    }
    [Fact]
    public async Task Right_OnSuccess_InvokesWrite()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        var sut=new DiffController(logger,mockDataService.Object,DiffController);;
        //Act
        var result=await sut.right("id1");
        //Assert
        mockDataService.Verify(service=>service.Write(It.IsAny<LeftRightDiff>()),Times.Once());
    }
    [Fact]
    public async Task diff_OnSuccess_InvokesReadLRD()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns( Task.FromResult<LeftRightDiff?>(new LeftRightDiff{Left="l",Right="r",ID="id1"}));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase=true;
        var sut=new DiffController(logger,mockDataService.Object,mockDiffService.Object);
        //Act
        var result=(OkObjectResult)(await sut.diff("id1"));
        //Assert
		mockDataService.Verify(service=>service.ReadLRD(It.IsAny<string>()),Times.Once());
    }
     [Fact]
    public async Task diff_OnSuccess_InvokesCalculateDiff()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.Write(It.IsAny<LeftRightDiff>()));
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns( Task.FromResult<LeftRightDiff?>(new LeftRightDiff{Left="l",Right="r",ID="id1"}));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase=true;
        var sut=new DiffController(logger,mockDataService.Object,mockDiffService.Object);
        //Act
        var result=(OkObjectResult)(await sut.diff("id1"));
        //Assert
		mockDiffService.Verify(service=>service.CalculateDiff(It.IsAny<string>()),Times.Once());
    }
}