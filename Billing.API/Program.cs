using Billing.API;
using Billing.API.Services;
using Billing.BLL.DataManagement;
using Billing.BLL.DataManagement.Interfaces;
using Billing.BLL.Helpers;
using Billing.BLL.Helpers.Interfaces;
using Billing.DAL.Contexts;
using Billing.DAL.Repositories.EFCore;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<BillingContext>(opt => opt.UseInMemoryDatabase("Billing"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersManagement, UsersManagement>();
builder.Services.AddScoped<ICoinsManagement, CoinsManagement>();
builder.Services.AddScoped<ICalculatingRewards, CalculatingRewards>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BillingContext>();

    DataGenerator.Initialize(context);
}
// Configure the HTTP request pipeline.
app.MapGrpcService<BillingService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();