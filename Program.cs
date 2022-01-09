using System.Runtime.CompilerServices;
using AuthService.JwtHelper;
using JWT.Algorithms;
using JWT.Builder;
using Models;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "All",
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.AllowAnyOrigin();
        });
});


var app = builder.Build();


app.UseCors("All");

app.MapGet("/", () => "Hello World!");


app.MapGet("/api/user", (JwtGetContext context) =>
{
    if (context.CurrentUser != null && context.CurrentUser.isAdmin())
    {
        var users = Users.GetUsers();
        var json = JsonConvert.SerializeObject(users);
        
        context.Current.Response.StatusCode = StatusCodes.Status200OK;
        return json;
    }
    else
    {
        context.Current.Response.StatusCode = StatusCodes.Status403Forbidden;
        return JsonConvert.SerializeObject(null);
    }
});


app.MapPost("/api/authorize", async (HttpContext http) =>
{
    var authData = await http.Request.ReadFromJsonAsync<AuthData>();

    var user = Users.GetUserByEmail(authData!.Email);
    var token = user!.GetJWT(authData!.Password) ?? "";

    http.Response.StatusCode = string.IsNullOrWhiteSpace(token)
        ? StatusCodes.Status401Unauthorized
        : StatusCodes.Status200OK;

    return token;
});

app.Run();