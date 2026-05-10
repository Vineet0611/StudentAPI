namespace Core.Configs
{
    public class AuthErrorResponseModel
    {
        public int status { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string correlationId { get; set; }
    }
}
