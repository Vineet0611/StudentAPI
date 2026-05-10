using Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GetAccessToken(Guid userId, SessionType sessionType);
    }
}
