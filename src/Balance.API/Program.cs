using Balance.Domain.ValueObjects;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapGet("/balances", () =>
{
    var balance = new Balance.Domain.Balance(interest: 800, principal: 200, operationDate: DateTime.UtcNow, bookingDate: DateTime.UtcNow);

    var paymentOriginGuid = Guid.CreateVersion7();
    balance.RegisterTransaction(paymentOriginGuid, TransactionType.Payment, 100, DateTime.UtcNow, DateTime.UtcNow);

    var correctionOriginGuid = Guid.CreateVersion7();
    balance.CompensateTransaction(paymentOriginGuid, DateTime.UtcNow);


    return Results.Ok();
})
.WithName("test");

app.MapScalarApiReference();

app.Run();