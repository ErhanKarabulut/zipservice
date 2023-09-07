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

app.MapGet("/{files}", async (int files, CancellationToken cancellationToken, HttpResponse response) =>
{
    using var memoryStream = new MemoryStream();
    await Zip.WriteAsync(memoryStream, files, cancellationToken);
    memoryStream.Position = 0;
    response.Headers.Add("Content-Disposition", "attachment; filename=test.zip");
    response.ContentType = "application/octet-stream";
    await memoryStream.CopyToAsync(response.Body, cancellationToken);
})
.WithOpenApi();

app.MapGet("/v2/{files}", async (int files, HttpResponse response) =>
{
    using var memoryStream = new MemoryStream();
    Zip.Write(memoryStream, files);
    memoryStream.Position = 0;
    response.Headers.Add("Content-Disposition", "attachment; filename=test.zip");
    response.ContentType = "application/octet-stream";
    await memoryStream.CopyToAsync(response.Body);
})
.WithOpenApi();

app.Run();

public partial class Program
{
}

