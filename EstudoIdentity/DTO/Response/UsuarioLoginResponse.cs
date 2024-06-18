﻿using System.Text.Json.Serialization;

namespace EstudoIdentity.DTO.Response;

public class UsuarioLoginResponse
{
    public bool Sucesso { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    
    // Token JWT
    public string Token { get; private set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    // Expiração do token
    public DateTime? DataExpiracao{ get; set; }
    public List<string> Errors { get; private set; }


    public UsuarioLoginResponse() => Errors = new List<string>();

    public UsuarioLoginResponse(bool sucesso = true): this()=>Sucesso = sucesso;
    public UsuarioLoginResponse(bool sucesso, string token, DateTime dataExpiracao): this()
    {
        Token = token;
        DataExpiracao = dataExpiracao;
    }

    public void AdicionarErros(IEnumerable<string> erros) => Errors.AddRange(erros);
    public void AdicionarErro(string erro) => Errors.Add(erro);


}
