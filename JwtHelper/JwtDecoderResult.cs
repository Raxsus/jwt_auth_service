namespace AuthService.JwtHelper;

public class JwtDecoderResult<T>
{
    public int Status { get; private set; }
    public T Data { get; private set; }

    public static JwtDecoderResult<T> Success(T result)
    {
        return new JwtDecoderResult<T>()
        {
            Status = 0,
            Data = result
        };
    }

    public static JwtDecoderResult<T> TokenExpired()
    {
        return new JwtDecoderResult<T>()
        {
            Status = 1
        };
    }

    public static JwtDecoderResult<T> SignatureInvalid()
    {
        return new JwtDecoderResult<T>()
        {
            Status = 2
        };
    }
    
    
}