using DAL004;
using Microsoft.AspNetCore.Diagnostics;
using System;


public class UpdateByIdException : Exception
{
    public UpdateByIdException(string message) : base($"Update by Id: {message}") { }
}
public class DeleteByIdException : Exception
{
    public DeleteByIdException(string message) : base($"Delete by Id: {message}") { }
}
public class FoundByIdException : Exception
{
    public FoundByIdException(string message) : base($"Found by Id: {message}") { }
}

public class SaveException : Exception
{
    public SaveException(string message) : base($"SaveChanges error: {message}") { }
}

public class AddCelebrityException : Exception
{
    public AddCelebrityException(string message) : base($"AddCelebrityException error: {message}") { }
}

public class HasCelebrityException : Exception
{
    public HasCelebrityException(string message) : base($"HasCelebrityException error: {message}") { }
}

public class Programm
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        using (IRepository repository = Repository.Create("Celebrities"))
        {
            app.UseExceptionHandler("/Celebrities/Error");

            app.MapGet("/Celebrities", () => repository.getAllCelebrities());

            app.MapGet("/Celebrities/{id:int}", (int id) =>
            {
                Celebrity? celebrity = repository.getCelebrityByid(id);
                if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
                return celebrity;
            });

            app.MapPost("/Celebrities", (Celebrity celebrity) =>
            {
                int? id = repository.addCelebrity(celebrity);
                List<string?> fileNames = Directory.GetFiles(@"C:\Users\user\source\repos\ASPA\ASPA004_3\Photo")
                                     .Select(Path.GetFileName)
                                     .ToList();
                if (!fileNames.Contains(celebrity.PhotoPath)) throw new HasCelebrityException("/Celebrities error, havent photo in directory");
                if (id == null) throw new AddCelebrityException("/Celebrities error, id == null");
                if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
            });

            app.MapDelete("/Celebrities/{id:int}", (int id) =>
            {
                bool CheckDelete = repository.delCelebtrityById(id);
                if (!CheckDelete) throw new DeleteByIdException($"DELETE /Celebrities error, id = {id}");
                if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                return $"Celebrity with Id = {id} deleted";
            });

            app.MapPut("/Celebrities/{id:int}", (int id, Celebrity celebrity) =>
            {
                bool CheckUpdate = repository.updCelebrityById(id, celebrity);
                if (!CheckUpdate) throw new UpdateByIdException($"UPDATE  /Celebrities error, id = {id}");
                if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                return celebrity;
            });

            app.MapFallback((HttpContext ctx) => Results.NotFound(new { error = $"{ctx.Request.Path} not supported" }));

            app.Map("/Celebrities/Error", (HttpContext ctx) =>
            {
                Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
                IResult rc = Results.Problem(detail: ex.Message, instance: app.Environment.EnvironmentName, title: "ASPA004", statusCode: 500);

                if (ex != null)
                {
                    if(ex is HasCelebrityException) rc = Results.Problem(title: "ASPA004/Directory", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404);
                    if (ex is UpdateByIdException) rc = Results.Problem(title: "ASPA004/Update", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404);
                    if (ex is DeleteByIdException) rc = Results.Problem(title: "ASPA004/Delete", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404);
                    if (ex is FoundByIdException) rc = Results.Problem(title: "ASPA004/Found", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404); // 404 - не найден
                    if (ex is BadHttpRequestException) rc = Results.Problem(title: "ASPA004/BadRequest", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404);
                    if (ex is SaveException) rc = Results.Problem(title: "ASPA004/SaveChanges", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                    if (ex is AddCelebrityException) rc = Results.Problem(title: "ASPA004/addCelebrity", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 500);
                }
                return rc;
            });

            app.Run();
        }
    }
}

