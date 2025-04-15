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

public class NullFieldAddException : Exception
{
    public NullFieldAddException(string message) : base($"NullFieldAddException error: {message}") { }
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
              
                if (repository.SaveChanges() <= 0) throw new SaveException("/Celebrities error, SaveChanges() <= 0");
                return new Celebrity((int)id, celebrity.Firstname, celebrity.Surname, celebrity.PhotoPath);
            }).AddEndpointFilter(async (context, next) =>
            {
                //если параметр celebrity == null, то 500 с пояснением
                //Если Surname == null или его длина < 2, то 409 с пояснениями

                Celebrity? celebrity = context.Arguments.OfType<Celebrity>().FirstOrDefault();

                if(celebrity == null) throw new AddCelebrityException("/Celebrities error, id == null");
                if(celebrity.Surname == null) throw new NullFieldAddException("/Celebrities error, Surname == null");
                if (celebrity.Firstname == null) throw new AddCelebrityException("/Celebrities error, Firstname == null");
                if (celebrity.PhotoPath == null) throw new AddCelebrityException("/Celebrities error, PhotoPath == null");
                if(celebrity.Surname.Length < 2) throw new NullFieldAddException("/Celebrities error, Surname is wrong");

                return next(context);
            }).AddEndpointFilter(async (context, next) =>
            {
                //если параметр celebrity == null, то 500 с пояснением
                //если есть уже такой Surname, то 409 с пояснениями
                Celebrity? celebrity = context.Arguments.OfType<Celebrity>().FirstOrDefault();

                if (celebrity == null) throw new AddCelebrityException("/Celebrities error, id == null");
                if (celebrity.Surname == null) throw new AddCelebrityException("/Celebrities error, Surname == null");
                if (celebrity.Firstname == null) throw new AddCelebrityException("/Celebrities error, Firstname == null");
                if (celebrity.PhotoPath == null) throw new AddCelebrityException("/Celebrities error, PhotoPath == null");
                if ((repository.getAllCelebrities().Select(e => e.Surname).ToList()).Contains(celebrity.Surname)) throw new NullFieldAddException("/Celebrities error, Surname is doubled");
                return next(context);
            }).AddEndpointFilter(async (context, next) =>
            {
                //если параметр celebrity == null, то 500 с пояснением
                //если нет фото по BasePath + вывод названия файла NotFound
                HttpContext http = context.HttpContext;
                Celebrity? celebrity = context.Arguments.OfType<Celebrity>().FirstOrDefault();
                List<string?> fileNames = Directory.GetFiles(@"C:\Users\user\source\repos\ASPA\ASPA004_3\Photo")
                                     .Select(Path.GetFileName)
                                     .ToList();

                if (celebrity == null) throw new AddCelebrityException("/Celebrities error, id == null");
                if (celebrity.Surname == null) throw new NullFieldAddException("/Celebrities error, Surname == null");
                if (celebrity.Firstname == null) throw new AddCelebrityException("/Celebrities error, Firstname == null");
                if (celebrity.PhotoPath == null) throw new AddCelebrityException("/Celebrities error, PhotoPath == null");
                if (celebrity.Surname.Length < 2) throw new NullFieldAddException("/Celebrities error, Surname is wrong");
                if (!fileNames.Contains(celebrity.PhotoPath))
                {
                    http.Response.Headers.Add("X-Celebrity", $"NotFound = {celebrity.PhotoPath}");
                }

                return next(context);
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
                    if(ex is NullFieldAddException) rc = Results.Problem(title: "ASPA004/Surname", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 409);
                    if (ex is UpdateByIdException) rc = Results.Problem(title: "ASPA004/Update", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404);
                    if (ex is HasCelebrityException) rc = Results.Problem(title: "ASPA004/Directory", detail: ex.Message, instance: app.Environment.EnvironmentName, statusCode: 404);
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

