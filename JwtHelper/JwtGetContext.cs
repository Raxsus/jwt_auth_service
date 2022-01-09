using Models;
using Newtonsoft.Json;

namespace AuthService.JwtHelper;

public class JwtGetContext
{
    public static ValueTask<JwtGetContext?> BindAsync(HttpContext context)
    {
        
        var parameters = context.Request.Query.Keys.Cast<string>()
            .ToDictionary(k => k, v => context.Request.Query[v].ToString());
        
        if (context.Request.Headers.TryGetValue("jwt", out var token))
        {
            var result = Jwt.Decode<User>(token);
            if (result.Status == 0)
            {
                context.Response.StatusCode = StatusCodes.Status200OK;
                return ValueTask.FromResult<JwtGetContext?>(new JwtGetContext()
                {
                    Parameters = parameters,
                    CurrentUser = result.Data,
                    Current = context
                });
            }
            else if (result.Status == 1 || result.Status == 2)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
        } 
        
        return ValueTask.FromResult<JwtGetContext?>(new JwtGetContext()
        {
            Parameters = parameters,
            CurrentUser = null,
            Current = context
        });
     }

    public Dictionary<string, string> Parameters { get; private set; }
    
    public User? CurrentUser { get; private set; }
    
    public HttpContext Current { get; private set; }
}