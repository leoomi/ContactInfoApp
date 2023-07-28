using Microsoft.AspNetCore.Mvc;

public class MediatorResult<T>
{
    public bool IsSuccess { get; } = false;
    public MediatorError Error { get; }
    public T? Value { get; }

    public MediatorResult(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    public MediatorResult(MediatorError error)
    {
        Error = error;
    }

    public ActionResult ReturnMediatorResultError()
    {
        return Error switch
        {
            MediatorError.Unauthorized => new UnauthorizedResult(),
            MediatorError.NotFound => new NotFoundResult(),
            MediatorError.BadRequest => new BadRequestResult(),
            _ => new StatusCodeResult(StatusCodes.Status500InternalServerError),
        };
    }
}