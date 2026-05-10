using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure.Header
{
    public class ApiResponseModel<T> : BaseResponseModel
    {
        public T data { get; set; }
    }
}
