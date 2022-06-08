using System.Security.Claims;
using System.Security.Principal;

namespace Digbyswift.Web.Extensions
{
    public static class PrincipalExtensions
    {
        public static bool IsLoggedIn(this IPrincipal user)
        {
            var identity = user?.Identity;
            return identity?.IsAuthenticated ?? false; 
        }
        
        public static bool HasRole(this IPrincipal user, string roleClaimValue)
        {
            if (!user.IsLoggedIn())
                return false;

            if (roleClaimValue == null || !(user?.Identity is ClaimsIdentity claimsIdentity))
                return false;
            
            return claimsIdentity.HasClaim(ClaimTypes.Role, roleClaimValue);

        }
    }
}