using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Payroll.Handlers;
using Repository.Common;
using Repository.Designation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://payrollweb-auhyawetasefa9a5.centralindia-01.azurewebsites.net"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();

// ---------------------------------------------------------
// HTTP pipeline
// ---------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("corsapp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.MapGet("/hello/test", (HttpContext context) =>
//{
//    return Results.Json(new
//    {
//        Content = "Hello From C#",
//        Path = context.Request.Path,
//        RunAt = DateTime.UtcNow
//    });
//});


app.Run();