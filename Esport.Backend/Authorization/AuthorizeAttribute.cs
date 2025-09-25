using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Esport.Backend.Entities;
using Esport.Backend.Enums;

namespace Esport.Backend.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<UserRole> _roles;

        public AuthorizeAttribute(params UserRole[] roles)
        {
            _roles = roles ?? new UserRole[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var user = (AuthUser)context.HttpContext.Items["User"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            var roleMatch = user?.Roles.Select(x => x.Role).ToList().Any(u => _roles.Contains(u));
            if (user == null || roleMatch == false)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}