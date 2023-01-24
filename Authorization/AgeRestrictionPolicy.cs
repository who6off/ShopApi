using Microsoft.AspNetCore.Authorization;

namespace ShopApi.Authorization
{
    public class AgeRestrictionPolicy : IAuthorizationRequirement
    {
        public const string Name = "AgeRestriction";
        public int PurmittedAge { get; set; }

        public AgeRestrictionPolicy(int age) => PurmittedAge = age;
    }
}
