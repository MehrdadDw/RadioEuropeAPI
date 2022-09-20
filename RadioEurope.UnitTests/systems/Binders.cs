using RadioEurope.API.Binders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Text;
namespace RadioEurope.UnitTests.systems;

public class Binders
{
    [Fact]
  public async Task Binder_Should_Bind()
    {
        // Arrange
            var modelMetadata = new EmptyModelMetadataProvider();

            var requestFake = new HttpRequestFeature();
            requestFake.QueryString = "?ID=arg1";
            requestFake.Body =new MemoryStream( Encoding.UTF8.GetBytes("eyJpbnB1dCI6InNvbWUgdmFsdWUgdG8gYmUgY29tcGFyZWQifQ==" ?? string.Empty));

            var features = new FeatureCollection();
            features.Set<IHttpRequestFeature>(requestFake);

            var fakeHttpContext = new DefaultHttpContext(features);

            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "CustomBinder",
                ModelMetadata = modelMetadata.GetMetadataForType(typeof(CustomBinder)),
                ActionContext = new ActionContext()
                {
                    HttpContext = fakeHttpContext
                }
            };
            var binder = new CustomBinder();
            //--- act
            await binder.BindModelAsync(bindingContext);

            //--- assert
            Assert.NotSame(null,bindingContext.Result);
            Assert.True(bindingContext.Result.IsModelSet);
    }
    [Theory]
    [InlineData("eyJpbnB1dCI6InNvbWUgdmFsdWUgdG8gYmUgY29tcGFyZWQifQ==", "some value to be compared")]
    [InlineData("eyJpbnB1dCI6ImJqanJwcXZmcEVHcWxQUHFVUXh6c1ZCWE9yU0hzQkhKUVNxeUF0VGd4RnNoUWlWU3Zxc0JZTkZOb09OIn0=", "bjjrpqvfpEGqlPPqUQxzsVBXOrSHsBHJQSqyAtTgxFshQiVSvqsBYNFNoON")]
    [InlineData("eyJpbnB1dCI6IlZhbHVlMSJ9", "Value1")]
    [InlineData("\\\"eyJpbnB1dCI6InRlc3RWYWx1ZSJ9\\\"", "testValue")]

    public async Task Binder_Should_CorrectlyDecode(string encoded,string expected)
    {
        // Arrange
            var modelMetadata = new EmptyModelMetadataProvider();

            var requestFake = new HttpRequestFeature();
            requestFake.QueryString = "?ID=arg1";
            requestFake.Body =new MemoryStream( Encoding.UTF8.GetBytes(encoded ?? string.Empty));

            var features = new FeatureCollection();
            features.Set<IHttpRequestFeature>(requestFake);

            var fakeHttpContext = new DefaultHttpContext(features);
            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "CustomBinder",
                ModelMetadata = modelMetadata.GetMetadataForType(typeof(CustomBinder)),
                ActionContext = new ActionContext()
                {
                    HttpContext = fakeHttpContext
                }
            };
            var binder = new CustomBinder();

            //--- act
            await binder.BindModelAsync(bindingContext);

            //--- assert
            Assert.Equal(bindingContext.Result.Model.ToString(),expected);

    }
}