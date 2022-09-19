using RadioEurope.API.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder.Services);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    //c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {

app.UseSwagger();
app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<IConnectionMultiplexer>
    (x => ConnectionMultiplexer.Connect("redis"));

    services.AddTransient<IDataService>
    (x => new DataService(x.GetRequiredService<IConnectionMultiplexer>()));

    services.AddTransient<IDiffService>
    (x => new DiffService(x.GetRequiredService<IDataService>()));
}