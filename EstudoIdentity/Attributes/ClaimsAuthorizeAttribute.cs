using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace EstudoIdentity.Attributes;

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter)) =>
        Arguments = new object[] { new Claim(claimType, claimValue) };
}

public class ClaimRequirementFilter : IAuthorizationFilter
{
    readonly Claim _claim;
    public ClaimRequirementFilter(Claim claim) => _claim = claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // recupera o user do httpcontexts e pega as claims deles
        var user = context.HttpContext.User as ClaimsPrincipal;
        
        // verifica se está autenticado
        if (user == null || !user.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        // se não tiver a claim, retorna um erro no context
        if (!user.HasClaim(_claim.Type, _claim.Value))
            context.Result = new ForbidResult();
    }
}
