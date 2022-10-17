using HelloApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HelloApi.Authorization
{
    public class AgeRestrictionPolicyHandler : AuthorizationHandler<AgeRestrictionPolicy>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AgeRestrictionPolicy requirement)
        {

            var birthDate = context.User?.FindFirst(c => c.Type == ClaimTypes.DateOfBirth)?.Value;
            if (birthDate is null) return Task.CompletedTask;

            var userAge = DateTime.Now.GetYearDifference(birthDate);

            if (userAge >= requirement.PurmittedAge)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
