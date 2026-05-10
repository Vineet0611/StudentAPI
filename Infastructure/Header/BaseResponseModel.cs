
namespace Infastructure.Header
{
    public class BaseResponseModel
    {
        public int status { get; set; } = 200;
        public String message { get; set; } = "OK";
        public String type { get; set; } = "API";
        public string CorrelationId { get; set; }


    }
}
