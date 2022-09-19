using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using RadioEurope.Utilities;
using RadioEurope.API.Models;
namespace RadioEurope.API.Binders;

public class CustomBinder : IModelBinder
{
    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {

 string model;
   string bodyAsText = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        
        var jsonString = bodyAsText.DecodeBase64();
        model=JsonSerializer.Deserialize<Input>(jsonString)?.input ?? "";
        bindingContext.Result = ModelBindingResult.Success(model);
    }
}
