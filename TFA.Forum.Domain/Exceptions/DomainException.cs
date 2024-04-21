namespace TFA.Forum.Domain.Exceptions;

public class DomainException(int statusCode, string message) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}