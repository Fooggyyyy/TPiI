using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

DefaultFilesOptions options = new DefaultFilesOptions();
options.DefaultFileNames.Clear();
options.DefaultFileNames.Add("Neyman.html");
app.UseDefaultFiles(options);

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot")),

    RequestPath = new PathString("/static")
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Picture")),

    RequestPath = new PathString("/staticPicture")
});

app.UseStaticFiles();
    
app.UseWelcomePage("/aspnetcore");

app.MapGet("/aspnetcore", () => "Не достижимо");

app.Run();
