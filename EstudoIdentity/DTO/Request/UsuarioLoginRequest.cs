using System.ComponentModel.DataAnnotations;

namespace EstudoIdentity.DTO.Request;

public class UsuarioLoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    public string Senha { get; set; }
}
