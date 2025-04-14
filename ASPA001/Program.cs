internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestMethod;
            Console.WriteLine("_______________________________HTTP LOGGING, ����: ________________________________________");
        });

        var app = builder.Build();

        app.UseHttpLogging();

        app.MapGet("/", () => "��� ������ ASPA!");
        app.MapGet("/Hello", () => "������������ ��������� ��������");

        app.Run();
    }
}