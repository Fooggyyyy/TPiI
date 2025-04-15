using DAL004;
using Microsoft.AspNetCore.Diagnostics;
using System;


namespace ASPA005_02
{
    public static class Validation
    {
        public class PhotoExistFilter : IEndpointFilter
        {
            public static IRepository Repositoryy { get; set; }
            public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
            {
                using (Repositoryy = Repository.Create("Celebrities"))
                {
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
                }
            }
        }

        public class SurnameFilter : IEndpointFilter
        {
            public static IRepository Repositoryy { get; set; }
            public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
            {

                using (Repositoryy = Repository.Create("Celebrities"))
                {
                    Celebrity? celebrity = context.Arguments.OfType<Celebrity>().FirstOrDefault();

                    if (celebrity == null) throw new AddCelebrityException("/Celebrities error, id == null");
                    if (celebrity.Surname == null) throw new NullFieldAddException("/Celebrities error, Surname == null");
                    if (celebrity.Firstname == null) throw new AddCelebrityException("/Celebrities error, Firstname == null");
                    if (celebrity.PhotoPath == null) throw new AddCelebrityException("/Celebrities error, PhotoPath == null");
                    if (celebrity.Surname.Length < 2) throw new NullFieldAddException("/Celebrities error, Surname is wrong");
                    if ((Repositoryy.getAllCelebrities().Select(e => e.Surname).ToList()).Contains(celebrity.Surname)) throw new NullFieldAddException("/Celebrities error, Surname is doubled");
                    return next(context);
                }
            }
        }
    }
}
