using Microsoft.AspNetCore.Http.Features;
using ZipService.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/{files}", async (int files, CancellationToken cancellationToken, HttpContext httpContext, HttpResponse response) =>
{
    httpContext.Features.Get<IHttpBodyControlFeature>()!.AllowSynchronousIO = true;

    response.Headers.Add("Content-Disposition", "attachment; filename=test.zip");
    response.ContentType = "application/octet-stream";

    await Zip.WriteAsync(response.Body, files, cancellationToken);

}).WithOpenApi();

app.MapGet("/v2/{files}", (int files, HttpContext httpContext, HttpResponse response) =>
{
    httpContext.Features.Get<IHttpBodyControlFeature>()!.AllowSynchronousIO = true;

    response.Headers.Add("Content-Disposition", "attachment; filename=test.zip");
    response.ContentType = "application/octet-stream";

    Zip.Write(response.Body, files);

}).WithOpenApi();

app.Run();

public partial class Program
{
}

