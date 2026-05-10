using Cortex.Mediator.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentApi.Features.AuthFeatures.Login
{
    public class LoginHandler : ICommandHandler<LoginRequestModel, LoginResponseModel>
    {
        private readonly IConfiguration _configuration;

        public LoginHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LoginResponseModel> Handle(LoginRequestModel request, CancellationToken cancellationToken)
        {
            var response = new LoginResponseModel();

            // Static credentials check
            if (request.Username != "admin" || request.Password != "password123")
            {
                response.status = 401;
                response.message = "Invalid username or password.";
                return response;
            }

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:audience"];
            var key = _configuration["Jwt:Key"];
            var expiryMinutesStr = _configuration["Jwt:MobileAppExpiryMinutes"];
            
            if (string.IsNullOrEmpty(key))
            {
                response.status = 500;
                response.message = "JWT Key is not configured properly.";
                return response;
            }

            double expiryMinutes = 60; // default
            if (double.TryParse(expiryMinutesStr, out double configExpiry))
            {
                expiryMinutes = configExpiry;
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, request.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            response.Token = tokenHandler.WriteToken(token);
            response.status = 200;
            response.message = "Login successful.";

            return await Task.FromResult(response);
        }
    }
}
