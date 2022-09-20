using RadioEurope.API.Application.Services;
using StackExchange.Redis;
using RadioEurope.API.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

// Add services injection to the container.
void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IConnectionMultiplexer>
    (x => ConnectionMultiplexer.Connect("redis"));

    services.AddTransient<IDataService>
    (x => new DataService(x.GetRequiredService<IConnectionMultiplexer>()));

    services.AddTransient<IDiffService>
    (x => new DiffService(x.GetRequiredService<IDataService>()));
}