using DAL003;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles();
app.UseDefaultFiles();

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider("C:\\Users\\user\\source\\repos\\ASPA\\DAL003\\Celebrities"),

    RequestPath = new PathString("/Photo")
});

app.UseDirectoryBrowser(new DirectoryBrowserOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine("C:\\Users\\user\\source\\repos\\ASPA\\DAL003\\Celebrities")),
    RequestPath = "/Celebrities/download"
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine("C:\\Users\\user\\source\\repos\\ASPA\\DAL003\\Celebrities")),
    RequestPath = "/Celebrities/download",
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Content-Disposition", "attachment");
    }
});

using (IRepository repository = Repository.Create("Celebrities"))
{
    app.MapGet("/Celebrities", () => repository.getAllCelebrities());
    app.MapGet("/Celebrities/{id:int}", (int id) => repository.getCelebrityByid(id));
    app.MapGet("/Celebrities/BySurname/{surname}", (string surname) => repository.GetCelebrityBySurname(surname));
    app.MapGet("/Celebrities/PhotoPathById/{id:int}", (int id) => repository.getPhotoPathByid(id));
    app.MapGet("/", () => "Hello World!");
    app.Run();
}

/*
1.����������� ��������� ���������������� �� ���������� ����������� � ���������� ������, � �������� �������� ���������, � �������� ������������� ������ ����� ��������, ��������������� ������������������ � �������, � ��������� ����������.
2.	���������� �� ����
3.	��� ������� ������ ��� �������������� ������� ������.
4.	��������� �����, ����� ���� ������������ � ����������. 
5.	������������ � ������� �������������� ������ � ����� ���� ������(JSON, XML � �.�), �������������� � �������� �������.
6.	JSON (JavaScript Object Notation) � ��������� ������ ������ �������. ������ �������� JS-��������, �������������� ��� ������������ � �������������. 
*/