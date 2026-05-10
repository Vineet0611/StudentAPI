using System.Text.Json.Serialization;

namespace Util.Util.BaseFeatures
{
    public class BaseQueryRequestModel
    {
        public string? filter { get; set; }
        public int pageSize { get; set; } = 100;
        public int pageNumber { get; set; } = 1;
        public string? orderby { get; set; }
        [JsonIgnore]
        public bool exportToExcel { get; set; } = false;
    }
}
