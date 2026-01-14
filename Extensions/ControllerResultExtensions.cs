using Microsoft.AspNetCore.Mvc;

public static class ControllerResultExtensions
{
    public static IActionResult ToActionResult(this ControllerBase controller, ServiceResult result)
    {
        return result.Statuscode switch
        {
            400 => controller.BadRequest(result.Error),
            401 => controller.Unauthorized(result.Error),
            403 => controller.Forbid(),
            404 => controller.NotFound(result.Error),
            _ => controller.StatusCode(500, "Unexpected error"),
        };
    }
}
