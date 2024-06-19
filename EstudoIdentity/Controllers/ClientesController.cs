using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstudoIdentity.Controllers;

[ApiController]
[Route("clientes")]
[Authorize]
public class ClientesController : ControllerBase
{
    [HttpGet]
    public IActionResult Todos()
    {
        var clientes = new List<string>();
        clientes.Add("José");
        clientes.Add("Maria");
        clientes.Add("Tonho");
        clientes.Add("Souza");

        return Ok(clientes);
    }
}
