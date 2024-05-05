using Shared.Results;

namespace Shared.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(List<Error> errors)
    {
        Errors = errors;
    }

    public List<Error> Errors { get; }
}
