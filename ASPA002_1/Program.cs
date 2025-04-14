var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseWelcomePage("/aspnetcore");

app.MapGet("/aspnetcore", () => "Не достижимо");

app.MapGet("/ASPA002", async () =>
{
    string imagePath = "wwwroot/Neyman.jpg";
    if (File.Exists(imagePath))
    {
        var bytes = await File.ReadAllBytesAsync(imagePath);
        return Results.File(bytes, "image/jpeg");
    }
    return Results.NotFound("Файл не найден");
});

app.Run();
