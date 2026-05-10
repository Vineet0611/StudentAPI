using Core.Configs;
using Core.Enum;
using Infastructure.Constants;
using Infastructure.Repository.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Services.TokenService
{
    public class TokenService(AppSettings _config, UserManager<Users> _userManager, IRepository<Doctor> _doctorRepository, IRepository<Patient> _patientRepository, RoleManager<Role> _roleManager) : ITokenService
    {
        public async Task<string> GetAccessToken(Guid userId,SessionType sessionType)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new Exception("User not found");

            var role = await _userManager.GetRolesAsync(user);


            Guid? doctorId = null;
            Guid? patientId = null;

            if (role.Contains("Doctor"))
            {
                doctorId = await _doctorRepository.GetAllQuery()
                                .Where(d => d.user_id == user.Id && d.status == EntityStatus.Active)
                                .Select(d => (Guid?)d.id)
                                .FirstOrDefaultAsync();
            }
            else if (role.Contains("Patient"))
            {
                patientId = await _patientRepository.GetAllQuery()
                                .Where(p => p.user_id == user.Id && p.status == EntityStatus.Active)
                                .Select(p => (Guid?)p.id)
                                .FirstOrDefaultAsync();
            }

            var roleClaims = new List<Claim>();
            foreach (var roleValue in role)
            {
                var roleName = await _roleManager.FindByNameAsync(roleValue);
                var claimsList = await _roleManager.GetClaimsAsync(roleName);
                roleClaims.AddRange(claimsList);
            }


            var expiryMinutes = sessionType.Equals(SessionType.Mobile) ? _config.Jwt.MobileAppExpiryMinutes : _config.Jwt.WebAppExpiryMinutes;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),

            };
            if (roleClaims != null)
            {
                foreach (var permission in roleClaims)
                {
                    claims.Add(new Claim("Permission", permission.Value));
                }
            }

            if (doctorId.HasValue)
                claims.Add(new Claim(CustomClaims.DoctorId, doctorId.Value.ToString()));

            if (patientId.HasValue)
                claims.Add(new Claim(CustomClaims.PatientId, patientId.Value.ToString()));
            if (user is not null)
                claims.Add(new Claim(CustomClaims.UserId, user.Id.ToString()));

            foreach (var roleName in role)
            {
                claims.Add(new Claim(CustomClaims.Role, roleName)); 
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.Jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _config.Jwt.Issuer,
                audience: _config.Jwt.Audience,
                claims: claims,
                expires: sessionType ==SessionType.Mobile  ?DateTime.Now.AddDays(expiryMinutes) :DateTime.Now.AddMinutes(expiryMinutes),
                signingCredentials: creds);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
