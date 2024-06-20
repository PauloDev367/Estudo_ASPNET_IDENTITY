using EstudoIdentity.Constants;
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

    /** 
        Para usar roles, primeiro é preciso cadastra-las na coluna AspNetRoles do banco de dados
        Para que um usuário tenha uma role, devemos inserir na tabela AspNetUserRoles, e refazer o login
     */
    //[Authorize(Roles = "Admin")]
    [Authorize(Roles = Roles.Admin)]
    [HttpGet("jose")]
    public IActionResult Jose()
    {
        var clientes = new List<string>();
        clientes.Add("José");
        clientes.Add("Maria");
        clientes.Add("Tonho");
        clientes.Add("Souza");

        return Ok(clientes);
    }
}
