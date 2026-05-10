
namespace Util.Services.Email
{
    public interface IEmailService
    {
        public void SendMail(string EmailDisplayName, string Subject, string From, string To, string HTMLContent);
    }
}
