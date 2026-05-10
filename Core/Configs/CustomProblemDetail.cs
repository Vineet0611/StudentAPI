using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configs
{
    public class CustomProblemDetail : ProblemDetails
    {
        public string CorrelationId { get; set; }
    }
}
