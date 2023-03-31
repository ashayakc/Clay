using System.Security.Claims;

namespace API.Authorization
{
    public class ClaimsHandler : IClaimsHandler
    {
        public long GetUserId(IEnumerable<Claim> claims)
        {
            var userIdClaim = claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.OrdinalIgnoreCase));
            if (userIdClaim != null)
            {
                return long.Parse(userIdClaim.Value);
            }
            return 0;
        }

        public long GetRoleId(IEnumerable<Claim> claims)
        {
            var roleIdClaim = claims.FirstOrDefault(x => x.Type.Equals("roleId", StringComparison.OrdinalIgnoreCase));
            if (roleIdClaim != null)
            {
                return long.Parse(roleIdClaim.Value);
            }
            return 0;
        }
    }
}
