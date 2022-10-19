using System.Security.Claims;

namespace HelloApi.Authorization
{
    public static class HttpContextUserExtentions
    {
        public static int? GetUserId(this HttpContext context)
        {
            var id = context.User?.FindFirst(c => c.Type == ClaimTypes.PrimarySid)?.Value;
            return (id is null) ? null : int.Parse(id);
        }
    }
}
