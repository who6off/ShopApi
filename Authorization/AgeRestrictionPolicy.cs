using Microsoft.AspNetCore.Authorization;

namespace HelloApi.Authorization
{
    public class AgeRestrictionPolicy : IAuthorizationRequirement
    {
        public const string Name = "AgeRestriction";
        public int PurmittedAge { get; set; }

        public AgeRestrictionPolicy(int age) => PurmittedAge = age;
    }
}
