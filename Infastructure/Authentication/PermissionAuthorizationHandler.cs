using Infastructure.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Authentication
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            string? memberId = context.User.Claims.FirstOrDefault(
             x => x.Type == CustomClaims.UserId)?.Value;

            if (!Guid.TryParse(memberId, out Guid parsedMemberId))
            {
                return;
            }

            using IServiceScope scope = _serviceScopeFactory.CreateScope();



            HashSet<string> permissions = GetPermissions(context.User);

            if (permissions.Contains(requirement.Permission.ToString()))
            {
                context.Succeed(requirement);
            }
        }

        private HashSet<string> GetPermissions(ClaimsPrincipal? principal)
        {
            if (principal == null)
            {
                return new HashSet<string>();
            }

            IEnumerable<Claim> permissionClaims = principal.FindAll(CustomClaims.Permission) ?? Enumerable.Empty<Claim>();
            return permissionClaims.Select(c => c.Value).ToHashSet();
        }

    }

}
