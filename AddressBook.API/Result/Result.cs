using Microsoft.AspNetCore.Mvc;

namespace AddressBook.API.Result;

public class Result<T>
{
    public ProblemDetails ProblemDetails { get; set; }
    public bool IsSuccess { get; set; }
    public List<string> ErrorMessages { get; set; } = new List<string>();
    public T Data { get; set; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { Data = data, IsSuccess = true };
    }

    public static Result<T> Failure(
        List<string> errorMessages,
        string detail = null,
        int statusCode = 400
    )
    {
        return new Result<T>
        {
            IsSuccess = false,
            ErrorMessages = errorMessages,
            ProblemDetails = new ProblemDetails
            {
                Detail = detail ?? string.Join(", ", errorMessages),
                Status = statusCode
            }
        };
    }
}
