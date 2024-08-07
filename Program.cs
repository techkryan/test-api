using Microsoft.EntityFrameworkCore;
using Test.Api.Endpoints;
using Test.Api.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("MusicCatalog");
builder.Services.AddDbContext<CatalogDbContext>(
    options =>
    {
        options.UseNpgsql(connectionString);
    });

var app = builder.Build();

app.MapAlbumsEndpoinsts();

app.Run();
