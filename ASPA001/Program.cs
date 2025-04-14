internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpLogging(options =>
        {
            options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.RequestMethod;
            Console.WriteLine("_______________________________HTTP LOGGING, логи: ________________________________________");
        });

        var app = builder.Build();

        app.UseHttpLogging();

        app.MapGet("/", () => "Мое первое ASPA!");
        app.MapGet("/Hello", () => "Здравствуйте Анастасия Павловна");

        app.Run();
    }
}