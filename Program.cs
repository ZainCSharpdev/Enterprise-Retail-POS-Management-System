using Dropbox.Api;
using Microsoft.EntityFrameworkCore;
using POSbackend.Models;
using POSbackend.Repository.implement.Bill;
using POSbackend.Repository.implement.Payment;
using POSbackend.Repository.implement.Product;
using POSbackend.Repository.implement.Sale;
using POSbackend.Repository.implement.Users;
using POSbackend.Repository.Interface.Bill;
using POSbackend.Repository.Interface.Payment;
using POSbackend.Repository.Interface.Product;
using POSbackend.Repository.Interface.Sales;
using POSbackend.Repository.Interface.Users;
using POSbackend.Service.implement.Bill;
using POSbackend.Service.implement.Payment;
using POSbackend.Service.implement.Product;
using POSbackend.Service.implement.Sale;
using POSbackend.Service.Interface.Bill;
using POSbackend.Service.Interface.Payment;
using POSbackend.Service.Interface.Products;
using POSbackend.Service.Interface.Sales;
using Scalar.AspNetCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ISaleRepo, SalesRepo>();
builder.Services.AddScoped<ISaleDetailRepo, SalesDetailRepo>();
builder.Services.AddScoped<IBillRepo, BillRepo>();
builder.Services.AddScoped<IPaymentRepo, PaymentRepo>();
builder.Services.AddScoped<IUserRepo,UserRepo>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISalesService, SalesSService>();
//builder.Services.AddScoped<IBillService, BillService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<DropboxClient>(sp =>
{
    var accessToken = builder.Configuration["Dropbox:AccessToken"];
    return new DropboxClient(accessToken);
});

builder.Services.AddDbContext<PosdbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Db")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy.WithOrigins("http://localhost:5173") // Vite default port
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
