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

            var birthDate = DateTime.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);
            var now = DateTime.Now;
            var userAge = now.Year - birthDate.Year;

            if ((now.Month < birthDate.Month) || (now.Month == birthDate.Month && now.Day < birthDate.Day))
                userAge--;

            if (userAge >= requirement.PurmittedAge)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
