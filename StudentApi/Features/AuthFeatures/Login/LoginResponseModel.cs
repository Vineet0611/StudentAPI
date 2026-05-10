using Infastructure.Header;

namespace StudentApi.Features.AuthFeatures.Login
{
    public class LoginResponseModel : BaseResponseModel
    {
        public string Token { get; set; }
    }
}
