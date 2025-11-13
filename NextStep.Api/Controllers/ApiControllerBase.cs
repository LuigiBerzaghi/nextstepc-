using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.Exceptions;

namespace NextStep.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected int GetUserId()
    {
        var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(claim, out var userId))
        {
            return userId;
        }

        throw new AppException("Token inválido ou ausente.", HttpStatusCode.Unauthorized);
    }
}
