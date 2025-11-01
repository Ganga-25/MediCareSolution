using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MediCare.Infrastructure.Extentions
{
    public static class ClaimPrinciples_Extensions
    {
        public static int GetUserId(this ClaimsPrincipal User)
        {
            var userIdClaim = User.FindFirst("userId");
            if(string.IsNullOrEmpty(userIdClaim?.Value)|| !int.TryParse(userIdClaim?.Value,out int userId)){
                throw new UnauthorizedAccessException("Invalid or missing claim");
            }
            return userId;
        }
        public static string GetUserRole(this ClaimsPrincipal User)
        {
            var UserRole = User.FindFirst("userRole");
            if (string.IsNullOrEmpty(UserRole?.Value))
            {
                throw new UnauthorizedAccessException("Invalid or missing claim.");
            }
            return UserRole.Value;
        }
    }
}
