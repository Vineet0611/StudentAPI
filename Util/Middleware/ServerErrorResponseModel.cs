namespace Util.Middleware
{
    public class ExceptionResponse
    {
        public string message { get; set; }
        public string inner { get; set; }
    }

    public class ServerErrorResponseModel
    {
        public int status { get; set; } = 500;
        public string type { get; set; }
        public string message { get; set; }
        public string correlationId { get; set; }
        public ExceptionResponse exception { get; set; }
    }
}
