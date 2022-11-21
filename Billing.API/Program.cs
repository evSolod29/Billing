using Billing.API;
using Billing.API.Services;
using Billing.BLL.DataManagement;
using Billing.BLL.DataManagement.Interfaces;
using Billing.DAL.Contexts;
using Billing.DAL.Repositories.EFCore;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<BillingContext>(opt => opt.UseInMemoryDatabase("Billing"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUsersManagement, UsersManagement>();
builder.Services.AddScoped<ICoinsManagement, CoinsManagement>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BillingContext>();

    DataGenerator.Initialize(context);
}

app.MapGrpcService<BillingService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();