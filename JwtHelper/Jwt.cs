using System.Reflection;
using JWT;
using JWT.Algorithms;
using JWT.Builder;
using JWT.Exceptions;
using Models;

namespace AuthService.JwtHelper;

public class Jwt
{
    private static readonly string secret = "CERTVERTV@#$FSDFFF";

    public static string? Encode<T>(T obj)
    {
        return JwtBuilder.Create()
            .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
            .WithSecret(secret)
            .AddClaim("user", obj)
            .Encode();
    }

    public static JwtDecoderResult<T> Decode<T>(string token)
    {
        try
        {
            var payload = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm()) // symmetric
                .WithSecret(secret)
                .MustVerifySignature()
                .Decode<IDictionary<string, T>>(token);

            return JwtDecoderResult<T>.Success(payload["user"]);
        }
        catch (TokenExpiredException)
        {
            return JwtDecoderResult<T>.TokenExpired();
        }
        catch (SignatureVerificationException)
        {
            return JwtDecoderResult<T>.SignatureInvalid();
        }
        catch (Exception e)
        {
            return JwtDecoderResult<T>.SignatureInvalid();
        }
    }
}