using System.Buffers.Text;
using JWT.Algorithms;
using JWT.Builder;

namespace Models;
public class User
{
    
    private static readonly string secret = "CERTVERTV@#$FSDFFF";
    public User(string firstName, string secondName, string email, string password, bool isAdmin = false)
    {
        Firstname = firstName;
        Secondname = secondName;
        Email = email;
        Password =  password;

        Role = isAdmin ? "Admin" : "User";
    }
    
    public string Firstname { get; set; }
    public string Secondname { get; set; }
    public string Email { get; set; }
    private string Password { get; }
    public string Role { get; set; }
    
    public bool CheckPassword(string password)
    {
        return password == Password;
    }

    public string? GetJWT(string password)
    {
        if (CheckPassword(password))
        {
            return JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                .WithSecret(secret)
                .AddClaim("user", this)
                .Encode();
        }

        return null;
    }

    public bool isAdmin()
    {
        return Role == "Admin";
    }
}