using Microsoft.AspNetCore.Http.Extensions;
using System.Web;
using TodoApi.StartupConfig;



var builder = WebApplication.CreateBuilder(args);

builder.AddStandardServices();
builder.AddAuthServices();
builder.AddHealthCheckServices();
builder.AddCustomServices();


var app = builder.Build();


// Example of routing a request (Youtube tutorial)   
// Endpoint task/4  - (.*) will take whatever after the slash and added to it to the url
// $1 will tak whatever in (.*)
// It didn't work/ It route the request from tasks/1 to todos/1 but it doesn't call the TodosController (the get method)
/*app.UseRewriter(new Microsoft.AspNetCore.Rewrite.RewriteOptions()
        .AddRedirect("tasks/(.*)", "todos/$1"));*/

/*var options = new Microsoft.AspNetCore.Rewrite.RewriteOptions()
        .AddRewrite("tasks/(.*)", "todos/$1", true);

app.UseRewriter(options);*/

/*app.Use(async (conext, next) =>
{
    var url = conext.Request.Path.Value;

    string[] pathValue = url.Split("/");
    var uriParam = pathValue[pathValue.Length - 1];

    Regex regex = new Regex(@"^\d$");

    if (url!.Contains("tasks") && regex.IsMatch(uriParam))
    {
        conext.Request.Path = $"/api/todos/{uriParam}";
    }

    await next();
});*/



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Adding custom middlware to log the information before and after each endpoint request (Youtube tutorial)
app.Use(async (context, next) =>
{

    var displayURL = context.Request.HttpContext.Request.GetDisplayUrl();
    var parsedUrl = displayURL.Split('?')[1];

    var paramsCollection = HttpUtility.ParseQueryString(parsedUrl);

    foreach (var key in paramsCollection.AllKeys)
    {
        Console.WriteLine($"Key: {key} => Value: {paramsCollection[key]}");
    }

    string[] uriPath = context.Request.Path.Value.TrimStart('/').Split('/');
    var uriParm = uriPath[uriPath.Length - 1];
    //var typedHEader = context.Request.HttpContext.Request.GetTypedHeaders();
    //var headersValues = context.Request.HttpContext.Request.Headers.Values;
    //var bodyType = context.Request.HttpContext.Request.Body.GetType;
    //var routeValue = context.Request.HttpContext.GetRouteValue;
    string testContext = $"[{context.Request.Method} {context.Request.Path.Value} {DateTime.UtcNow}] Started.";
    //Console.WriteLine(testContext);
    app.Logger.LogInformation(testContext);
    await next(context);
    string testContextNext = $"[{context.Request.Method} {context.Request.Path.Value} {DateTime.UtcNow}] Finished.";
    //Console.WriteLine(testContextNext);
    app.Logger.LogInformation(testContextNext);

    //var testContextNext1 = context.Response.Headers.ToList();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();


app.Run();
