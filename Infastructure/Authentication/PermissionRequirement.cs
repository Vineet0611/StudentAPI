using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Authentication
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(string permission)
        {
            if (Enum.TryParse(typeof(Permissions), permission, true, out var result) &&
            Enum.IsDefined(typeof(Permissions), result))
            {
                Permission = (int)result;
            }
            else
            {
                Permission = 0;
            }
        }

        public int Permission { get; }
    }
}
