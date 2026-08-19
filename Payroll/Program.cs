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
    options.WithOrigins("http://localhost:4200", "https://payrollweb-auhyawetasefa9a5.centralindia-01.azurewebsites.net/")
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

app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> dataSources) =>
{
    var routes = dataSources
        .SelectMany(ds => ds.Endpoints)
        .OfType<RouteEndpoint>()
        .Select(e => new
        {
            Route = e.RoutePattern.RawText,
            DisplayName = e.DisplayName
        })
        .OrderBy(x => x.Route)
        .ToList();

    return Results.Ok(routes);
});


app.Run();