using EstudoIdentity.DTO.Request;
using EstudoIdentity.DTO.Response;
using EstudoIdentity.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstudoIdentity.Controllers;

[ApiController]
public class UsuarioController : ControllerBase
{
    private IdentityService _identityService;
    public UsuarioController(IdentityService identityService) => _identityService = identityService;

    [HttpPost("cadastro")]
    public async Task<IActionResult> Cadastrar(UsuarioCadastroRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await _identityService.CadastrarUsuario(request);

        if (resultado.Sucesso)
            return Ok(resultado);
        else if (resultado.Errors.Count > 0)
            return BadRequest(resultado);

        return StatusCode(500, new { error = "Internal server error" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UsuarioLoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var resultado = await _identityService.Login(request);
        if (resultado.Sucesso)
            return Ok(resultado);

        return Unauthorized(resultado);
    }

}
