using HelloApi.Extensions;
using System.Security.Claims;

namespace HelloApi.Authorization
{
    public static class ClaimsPrincipalExtentions
    {
        public static int? GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            return (id is null) ? null : int.Parse(id);
        }

        public static string? GetUserRole(this ClaimsPrincipal user)
        {
            var role = user.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
            return role;
        }

        public static int? GetUserAge(this ClaimsPrincipal user)
        {
            var birthDate = user.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)?.Value;
            if (birthDate is null)
                return null;

            var userAge = DateTime.Now.GetYearDifference(birthDate);
            return userAge;
        }
    }
}
