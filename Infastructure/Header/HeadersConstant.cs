using System.Runtime.InteropServices;

namespace Common.Constant
{
    public static class HeadersConstant
    {
        public const string correlationId = "X-CORRELATION-ID";


        //serilog TemplateName variable
        public const string serilogCorrelationId = "correlationId";
        public const string serilogResponseKey = "Response";
        public const string serilogRequestKey = "Request";
        public const string serilogActionName = "ActionName";
        public const string serilogActionId = "ActionId";


        public static List<CommonHeader> getAllHeaders()
        {
            List<CommonHeader> headersList = new List<CommonHeader>();
            headersList.Add(new CommonHeader(correlationId, false, getRandomGuid, serilogCorrelationId));
            return headersList;
        }

        public static string getRandomGuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static string getIgnore()
        {
            return string.Empty;
        }
    }

    public delegate double GeneratedValue();

    public class CommonHeader
    {
        public string Name { get; set; }
        public bool Required { get; set; }
        public string? TemplateName { get; set; }
        public Func<string> GeneratedValue { get; set; }
        public CommonHeader(string name, bool required, [Optional] Func<string> GeneratedValue, string? templateName = null)
        {
            this.Name = name;
            this.Required = required;
            this.GeneratedValue = GeneratedValue;
            this.TemplateName = templateName;
        }
    }
}
