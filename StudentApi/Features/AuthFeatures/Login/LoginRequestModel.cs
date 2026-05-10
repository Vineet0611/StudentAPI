using Cortex.Mediator.Commands;

namespace StudentApi.Features.AuthFeatures.Login
{
    public class LoginRequestModel : ICommand<LoginResponseModel>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
