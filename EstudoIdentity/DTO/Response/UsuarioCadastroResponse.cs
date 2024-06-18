namespace EstudoIdentity.DTO.Response;

public class UsuarioCadastroResponse
{
    public bool Sucesso { get; set; }
    public List<string> Errors { get; private set; }
    public UsuarioCadastroResponse() => Errors = new List<string>();

    public void AdicionarErros(IEnumerable<string> erros) => Errors.AddRange(erros);
    public UsuarioCadastroResponse(bool sucesso) => Sucesso = sucesso;

}
