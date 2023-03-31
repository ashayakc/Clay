using System.Security.Claims;

namespace API.Authorization
{
    public interface IClaimsHandler
    {
        long GetUserId(IEnumerable<Claim> claims);
        long GetRoleId(IEnumerable<Claim> claims);
    }
}
