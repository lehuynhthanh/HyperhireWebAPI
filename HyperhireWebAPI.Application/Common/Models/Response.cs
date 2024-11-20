namespace HyperhireWebAPI.Application.Common.Models;

public class Response
{
    public Response()
    {
        Succeeded = true;
        Errors = Array.Empty<string>();
    }
    internal Response(bool succeeded, string message, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
        Message = message;
    }

    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string[] Errors { get; set; }

    public static Response Success(string message = default!)
    {
        return new Response(true, message, Array.Empty<string>());
    }

    public static Response Failure(IEnumerable<string> errors, string message = default!)
    {
        return new Response(false, message, errors);
    }
}

public class Response<T> : Response
{
    public Response() : base()
    {

    }
    internal Response(bool succeeded, string message = default!, IEnumerable<string> errors = default!) : base(succeeded, message, errors)
    {
    }

    public static Response<T> Success(T data, string message = default!)
    {
        var r = new Response<T>(true, message, Array.Empty<string>())
        {
            Data = data
        };
        return r;
    }

    public static Response<T> Failure(T? data, IEnumerable<string> errors, string message = default!)
    {
        var r = new Response<T>(false, message, errors)
        {
            Data = data
        };
        return r;
    }

    public T? Data { get; set; }
}
