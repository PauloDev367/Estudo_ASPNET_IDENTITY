using EstudoIdentity.Attributes;
using EstudoIdentity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

    /**
        Para autenticação por claims, primeiro devemos inserir a claim na coluna AspNetUserClaims
        Por padrão, não existe uma anotação para trabalhar com claims, mas a solução adotada pela 
        comunidade é criar um atributo nosso para trabalhar com Claims, para facilitar na hora de colocar 
        nos métodos
     */
    [ClaimsAuthorize(Constants.ClaimTypes.Produto, "Ler")]
    [HttpGet("claim")]
    public IActionResult claimRota()
    {
        return Ok(new { name = "ABACATE" });
    }
}
