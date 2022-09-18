using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using RadioEurope.Helpers;
using RadioEurope.API.Models;
namespace RadioEurope.API.Binders;

public class CustomBinder : IModelBinder
{
    private readonly IModelBinderFactory factory;
    public CustomBinder(IModelBinderFactory factory) => this.factory = factory;

    public async Task BindModelAsync(ModelBindingContext bindingContext)
    {

 string model;
   string bodyAsText = await new StreamReader(bindingContext.HttpContext.Request.Body).ReadToEndAsync();
        
        var jsonString = bodyAsText.DecodeBase64();
        model=JsonSerializer.Deserialize<Input>(jsonString).input;
        bindingContext.Result = ModelBindingResult.Success(model);





        // var contentType =
        //     bindingContext.ActionContext.HttpContext.Request.ContentType;

        // BindingInfo bindingInfo = new BindingInfo();
        // if (contentType == "application/json")
        // {
        //     bindingInfo.BindingSource = BindingSource.Body;
        // }
        // else if (contentType == "application/x-www-form-urlencoded")
        // {
        //     bindingInfo.BindingSource = BindingSource.Form;
        // }
        //    else if (contentType == "application/custom")
        // {
        //     bindingInfo.BindingSource = BindingSource.Custom;
        // }
        // else
        // {
        //     bindingContext.Result = ModelBindingResult.Failed();
        // }

        // var binder = factory.CreateBinder(new ModelBinderFactoryContext
        // {
        //     Metadata = bindingContext.ModelMetadata,
        //     BindingInfo = bindingInfo,
        // });

        // await binder.BindModelAsync(bindingContext);
    }
}
