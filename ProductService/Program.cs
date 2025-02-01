using BasketService.MessagingBus.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProductService.Infrastructure;
using ProductService.MessageBus;
using ProductService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ProductDataBaseContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService.Services.ProductService>();
builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));
builder.Services.AddSingleton<IMessageRabbitHelper, RabbitMqMessageBusHelper>();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.Authority = "https://localhost:7032"; //Identity Server
        option.Audience = "productService";
    });

builder.Services.AddAuthorization(option =>
{
    option.AddPolicy("ProductForAdmin", p => p.RequireClaim("scope", "productServicePanel.FullAccess"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
