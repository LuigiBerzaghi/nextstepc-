using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NextStep.Application.DTOs.Profile;
using NextStep.Application.Interfaces.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace NextStep.Api.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/profile")]
[SwaggerTag("Gestão de perfil do usuário autenticado")]
public class ProfileController : ApiControllerBase
{
    private readonly IUserService _userService;

    public ProfileController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Obtém o perfil do usuário logado",
        Description = "Retorna email, nome, cargo atual, datas de criação e atualização."
    )]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var response = await _userService.GetProfileAsync(GetUserId(), cancellationToken);
        return Ok(response);
    }

    [HttpPut]
    [ProducesResponseType(typeof(ProfileResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Atualiza dados do perfil",
        Description = "Permite alterar o nome e o cargo atual do usuário autenticado."
    )]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
    {
        var response = await _userService.UpdateProfileAsync(GetUserId(), request, cancellationToken);
        return Ok(response);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerOperation(
        Summary = "Remove o usuário",
        Description = "Marca o usuário como removido e impede novos acessos."
    )]
    public async Task<IActionResult> Delete(CancellationToken cancellationToken)
    {
        await _userService.DeleteProfileAsync(GetUserId(), cancellationToken);
        return NoContent();
    }
}
