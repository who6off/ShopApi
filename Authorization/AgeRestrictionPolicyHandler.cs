using Microsoft.AspNetCore.Authorization;

namespace HelloApi.Authorization
{
    public class AgeRestrictionPolicyHandler : AuthorizationHandler<AgeRestrictionPolicy>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AgeRestrictionPolicy requirement)
        {

            var userAge = context.User?.GetUserAge();
            if (userAge is null) return Task.CompletedTask;

            if (userAge >= requirement.PurmittedAge)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
