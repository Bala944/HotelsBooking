using Microsoft.AspNetCore.Authorization;

namespace Booking.Models
{
    public static class Policies
    {
        public const string RequireAdminClaim = "RequireAdminClaim";

        public static AuthorizationPolicy RequireAdminClaimPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireClaim("Role", "Admin")
                .Build();
        }
    }
}
