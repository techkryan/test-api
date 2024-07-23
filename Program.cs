using Test.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapAlbumsEndpoinsts();

app.Run();
