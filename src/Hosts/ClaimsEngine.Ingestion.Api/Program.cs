using ClaimsEngine.Application;
using ClaimsEngine.Infra.Data;
using ClaimsEngine.Ingestion.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();

// API projects can reject HTTP requests rather than use UseHttpsRedirection
//app.UseHttpsRedirection();

app.UseRouting();
app.MapControllers();

app.Run();
