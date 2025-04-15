using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseExceptionHandler("/Error");

app.MapGet("/A/{x:int}", (HttpContext context, [FromRoute] int? x) =>
{
    if (x > 100)
        return Results.NotFound(new { message = $"path /A/{x} not supported" });

    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapPost("/A/{x:int}", (HttpContext context, [FromRoute] int x) =>
{
    if (x < 0 || x > 100)
        return Results.NotFound(new { message = $"path /A/{x} not supported" });

    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapPut("/A/{x:int}/{y:int}", (HttpContext context, [FromRoute] int x, [FromRoute] int y) =>
{
    if (x < 1 || y < 1)
        return Results.NotFound(new { message = $"path /A/{x}/{y} not supported" });

    return Results.Ok(new { path = context.Request.Path.Value, x = x, y = y });
});

app.MapDelete("/A/{x:int}-{y:int}", (HttpContext context, [FromRoute] int x, [FromRoute] int y) =>
{
    if (x < 1 || y < 1 || y > 100)
        return Results.NotFound(new { message = $"path /A/{x}/{y} not supported" });

    return Results.Ok(new { path = context.Request.Path.Value, x = x, y = y });
});


app.MapGet("/B/{x:float}", (HttpContext context, [FromRoute] float x) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapPost("/B/{x:float}/{y:float}", (HttpContext context, [FromRoute] float x, [FromRoute] float y) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x, y = y });
});

app.MapDelete("/B/{x:float}-{y:float}", (HttpContext context, [FromRoute] float x, [FromRoute] float y) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x, y = y });
});


app.MapGet("/C/{x:bool}", (HttpContext context, [FromRoute] bool x) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapPost("/C/{x:bool},{y:bool}", (HttpContext context, [FromRoute] bool x, [FromRoute] bool y) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x, y = y });
});


app.MapGet("/D/{x:datetime}", (HttpContext context, [FromRoute] DateTime x) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapPost("/D/{x:datetime}|{y:datetime}", (HttpContext context, [FromRoute] DateTime x, [FromRoute] DateTime y) =>
{
    return Results.Ok(new { path = context.Request.Path.Value, x = x, y = y });
});


app.MapGet("/E/12-{x}", (HttpContext context, [FromRoute] string x) =>
{
    if (string.IsNullOrEmpty(x))
        return Results.NotFound(new { message = $"path /E/{x} not supported" });

    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapPut("/E/{x}", (HttpContext context, [FromRoute] string x) =>
{
    if (!System.Text.RegularExpressions.Regex.IsMatch(x, @"^[a-zA-Z]{2,12}$"))
        return Results.NotFound(new { message = $"path /E/{x} not supported" });

    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});


app.MapGet("/F/{x}", (HttpContext context, [FromRoute] string x) =>
{
    if (!System.Text.RegularExpressions.Regex.IsMatch(x, @"^[^@\s]+@[^@\s]+\.by$"))
    {
        return Results.NotFound(new { message = $"path /F/{x} not supported" });
    }
    return Results.Ok(new { path = context.Request.Path.Value, x = x });
});

app.MapFallback((HttpContext ctx) =>
{
    return Results.NotFound(new { message = $"path {ctx.Request.Path.Value} not supported" });
});

app.Map("/Error", (HttpContext ctx) =>
{
    Exception? ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    return Results.Ok(new { message = ex?.Message });
});
app.Run();