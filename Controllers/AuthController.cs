using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController
{
    private readonly string secret = "CERTVERTV@#$FSDFFF";
    
    [HttpPost(Name = "auth")]
    public string? Auth([FromBody]JObject data)
    {
        var email = data["email"]!.ToString();
        var password = data["password"]!.ToString();
        string? token = null;
        
        if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(password))
        {
            var user = Users.GetUserByEmail(email);
            if (user != null && user.CheckPassword(password))
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
                token = JwtBuilder.Create()
                    .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                    .WithSecret(secret)
                    .AddClaim("user", json)
                    .Encode();
            }
        }

        return token;
    }
}