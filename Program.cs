using CurrencyApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<FrankfurterService>();
builder.Services.AddHttpClient<TcmbService>();
builder.Services.AddHttpClient<AltinkaynakService>();
// Add the services to the container
builder.Services.AddKeyedTransient<ICurrencyService>("Frankfurter", (sp, key) => sp.GetRequiredService<FrankfurterService>());
builder.Services.AddKeyedTransient<ICurrencyService>("Tcmb", (sp, key) => sp.GetRequiredService<TcmbService>());
builder.Services.AddKeyedTransient<ICurrencyService>("Altinkaynak", (sp, key) => sp.GetRequiredService<AltinkaynakService>());

// Build the app
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();