using Moq;
using RadioEurope.API.Application.Services;
using RadioEurope.API.Application.Interfaces;
using RadioEurope.API.Models;
using RadioEurope.API.Models.Enums;
using StackExchange.Redis;
namespace RadioEurope.UnitTests;

public class Services
{
    IConnectionMultiplexer multiplexer = null;

    [Theory]
    [InlineData("123456", "ab34ef", new object[] { new int[] { 0, 2 }, new int[] { 4, 2 } })]
    [InlineData("123456", "abcdef", new object[] { new int[] { 0, 6 } })]
    [InlineData("abc456", "abcdef", new object[] { new int[] { 3, 3 } })]
    [InlineData("abcefghij", "0b0e0g0i0", new object[] { new int[] { 0, 1 }, new int[] { 2, 1 }, new int[] { 4, 1 }, new int[] { 6, 1 }, new int[] { 8, 1 } })]
    [InlineData("NnbnYOcj", "NnISsWcF", new object[] { new int[] { 2, 4 }, new int[] { 7, 1 } })]
    public async void DiffService_Should_Correctly_Calculate(string l, string r, object[] answer)
    {

        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(new LeftRightDiff { Left = l, Right = r, ID = "id1" }));

        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");
        //Assert
        foreach (var item in answer)
        {
             Assert.Contains(new OffsetLength { offset = ((int[])item)[0], length = ((int[])item)[1]} ,DiffResult.Data);
        }
        Assert.Equal(answer.Length, DiffResult.Data.Count);
    }

    [Fact]
    public async void DiffService_Should_Detect_Equal()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(new LeftRightDiff { Left = "value1", Right = "value1", ID = "id1" }));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");

        //Assert
        Assert.Equal(DiffMessage.Equal,DiffResult.Message);
    }

    [Fact]
    public async void DiffService_Should_Detect_SizeDiff()
    {
        //Arrange
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(new LeftRightDiff { Left = "value", Right = "valuevalue", ID = "id1" }));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");

        //Assert		
        Assert.Equal(DiffMessage.LengthsNotEqual,DiffResult.Message);
    }

    [Fact]
    public async void DiffService_Should_Detect_NofFound()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(null));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");

        //Assert		
        Assert.Equal(DiffMessage.KeyNotFound,DiffResult.Message);
    }

    [Fact]
    public async void DiffService_Should_Return_Equal()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(new LeftRightDiff { Left = "QSqyA TgMFshQ", Right = "QSqyA TgMFshQ", ID = "id1" }));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");
        //Assert
        Assert.Equal(DiffMessage.Equal, DiffResult.Message);
    }
    [Fact]
    public async void DiffService_Should_Return_DifferentSize()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(new LeftRightDiff { Left = "QSqyA TgMFshQ", Right = "QSxxxxxxqyA TgMFshQ", ID = "id1" }));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");
        //Assert
        Assert.Equal(DiffMessage.LengthsNotEqual, DiffResult.Message);
    }
    [Fact]
    public async void DiffService_Should_Return_NotFound()
    {
        //Arrange
        IConnectionMultiplexer multiplexer = null;
        var mockDataService = new Mock<DataService>(multiplexer).As<IDataService>();
        mockDataService.CallBase = true;
        mockDataService.Setup(s => s.ReadLRD(It.IsAny<string>()))
        .Returns(Task.FromResult<LeftRightDiff?>(null));
        var mockDiffService = new Mock<DiffService>(mockDataService.Object).As<IDiffService>();
        mockDiffService.CallBase = true;
        //Act
        var DiffResult = await mockDiffService.Object.CalculateDiff("id1");
        //Assert
        Assert.Equal(DiffMessage.KeyNotFound, DiffResult.Message);
    }
}
