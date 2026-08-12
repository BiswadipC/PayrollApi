using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Common;
using Repository.Designation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });

builder.Services.AddCors(p => p.AddPolicy("corsapp", options =>
{
    options.WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials();
}));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PayrollContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("SqlConnection")));

builder.Services.AddDependency(builder.Configuration);

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
app.UseHttpsRedirection();
app.UseCors("corsapp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();