using ZipService.Api;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AllowSynchronousIO = true;
});

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

app.MapGet("/{files}", async (int files, CancellationToken cancellationToken, HttpResponse response) =>
{
    response.Headers.Add("Content-Disposition", "attachment; filename=test.zip");
    response.ContentType = "application/octet-stream";

    await Zip.WriteAsync(response.Body, files, cancellationToken);

}).WithOpenApi();

app.MapGet("/v2/{files}", (int files, HttpResponse response) =>
{
    response.Headers.Add("Content-Disposition", "attachment; filename=test.zip");
    response.ContentType = "application/octet-stream";

    Zip.Write(response.Body, files);

}).WithOpenApi();

app.Run();

public partial class Program
{
}

