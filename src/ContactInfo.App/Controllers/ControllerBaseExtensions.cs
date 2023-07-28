using Microsoft.AspNetCore.Mvc;

namespace ContactInfo.App.Controllers;

public static class ControllerBaseExtensions
{
    public static ActionResult<T> ReturnMediatorError<T>(this ControllerBase controller, MediatorError error)
    {
        return error switch
        {
            MediatorError.Unauthorized => (ActionResult<T>)controller.Unauthorized(),
            MediatorError.NotFound => (ActionResult<T>)controller.NotFound(),
            MediatorError.BadRequest => (ActionResult<T>)controller.BadRequest(),
            _ => (ActionResult<T>)controller.StatusCode(StatusCodes.Status500InternalServerError),
        };
    }
}