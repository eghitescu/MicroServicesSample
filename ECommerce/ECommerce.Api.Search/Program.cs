using ECommerce.Api.Search.Interfaces;
using ECommerce.Api.Search.Services;
using Microsoft.Extensions.DependencyInjection;

var configurationBuilder = new ConfigurationBuilder()
     .AddJsonFile($"appsettings.json");
var configuration = configurationBuilder.Build();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddHttpClient("OrdersService", config => 
{ 
    config.BaseAddress = new Uri(configuration["Services:Orders"]); 
});
builder.Services.AddHttpClient("ProductsService", config =>
{
    config.BaseAddress = new Uri(configuration["Services:Products"]);
});
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
