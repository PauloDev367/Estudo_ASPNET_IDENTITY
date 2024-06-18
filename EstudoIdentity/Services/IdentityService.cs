using EstudoIdentity.DTO.Request;
using EstudoIdentity.DTO.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EstudoIdentity.Services;

public class IdentityService
{
    // IdentityUser: Usuário padrão do identity
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager; // geralmente usado p/ gerenciamente de users
    private readonly JwtOptions _jwtOptions;

    public IdentityService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<JwtOptions> jwtOptions)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<UsuarioCadastroResponse> CadastrarUsuario(UsuarioCadastroRequest usuarioCadastro)
    {
        var identityUser = new IdentityUser
        {
            UserName = usuarioCadastro.Email,
            Email = usuarioCadastro.Email,
            EmailConfirmed = true // só isso não desbloqueia o usuário, precisa colocar o lockoutenable como false
        };
        var result = await _userManager.CreateAsync(identityUser, usuarioCadastro.Senha);
        if (result.Succeeded)
            await _userManager.SetLockoutEnabledAsync(identityUser, false); // desbloqueia o usuário

        var usuarioCadastroResponse = new UsuarioCadastroResponse(result.Succeeded);
        if (!result.Succeeded && result.Errors.Count() > 0)
            usuarioCadastroResponse.AdicionarErros(result.Errors.Select(r => r.Description));

        return usuarioCadastroResponse;
    }

    public async Task<UsuarioLoginResponse> Login(UsuarioLoginRequest usuarioLogin)
    {
        var result = await _signInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);
        //if(result.Succeeded)
        //    return await GerarToken(usuarioLogin.Email);
    
        var usuarioLoginResponse = new UsuarioLoginResponse(result.Succeeded);
        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
                usuarioLoginResponse.AdicionarErro("Conta bloqueada");
            else if (result.IsNotAllowed)
                usuarioLoginResponse.AdicionarErro("Essa conta não tem permissão para essa ação");
            else if (result.RequiresTwoFactor)
                usuarioLoginResponse.AdicionarErro("É necessário confirmar o login com o código de 2 fatores");
            else
                usuarioLoginResponse.AdicionarErro("Usuário ou senha inválidos");
        }
        return usuarioLoginResponse;
    }
}
