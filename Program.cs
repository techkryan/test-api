using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
// using Microsoft.AspNetCore.OpenApi;
// using Swashbuckle.AspNetCore;

using Test.Api.Endpoints;
using Test.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MusicCatalog API",
        Description = "An ASP.NET Core Web API for managing recordings' metadata",
        // TermsOfService = new Uri("https://example.com/terms"),
        // Contact = new OpenApiContact
        // {
        //     Name = "Example Contact",
        //     Url = new Uri("https://example.com/contact")
        // },
        // License = new OpenApiLicense
        // {
        //     Name = "Example License",
        //     Url = new Uri("https://example.com/license")
        // }
    });
});

var connectionString = builder.Configuration.GetConnectionString("MusicCatalog");

builder.Services.AddDbContext<CatalogDbContext>(
    options =>
    {
        options.UseNpgsql(connectionString);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.MapAlbumsEndpoinsts();

app.Run();
