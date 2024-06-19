using Microsoft.AspNetCore.Identity;

namespace EstudoIdentity.DTO.Response;

public class UsuarioCadastroResponse
{
    public bool Sucesso { get; set; }
    public List<string> Errors { get; private set; }

    public UsuarioCadastroResponse()
    {
        Errors = new List<string>();
    }

    public UsuarioCadastroResponse(bool sucesso) : this() // Chama o construtor padrão para inicializar a lista
    {
        Sucesso = sucesso;
    }

    public void AdicionarErros(IEnumerable<string> erros)
    {
        Errors.AddRange(erros);
    }

}
