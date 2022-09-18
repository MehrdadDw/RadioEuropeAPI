using RadioEurope.API.Binders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
using RadioEurope.Utilities;
namespace RadioEurope.UnitTests.systems;

public class EncodingTests
{
    [Theory]
    [InlineData("{\"input\":\"testValue\"}", "eyJpbnB1dCI6InRlc3RWYWx1ZSJ9")]
    [InlineData("some value", "c29tZSB2YWx1ZQ==")]

    public async Task Encoder_Should_CorrectlyEncode(string input,string expected)
    {
           // Arrange
 
            //--- act
            var result=input.EncodeBase64();

            //--- assert
            Assert.Equal(result,expected);

    }
    [Theory]
    [InlineData( "eyJpbnB1dCI6InRlc3RWYWx1ZSJ9","{\"input\":\"testValue\"}")]
    [InlineData( "c29tZSB2YWx1ZQ==","some value")]

    public async Task Decoder_Should_CorrectlyDecode(string input,string expected)
    {
           // Arrange
 
            //--- act
            var result=input.DecodeBase64();

            //--- assert
            Assert.Equal(result,expected);

    }
}