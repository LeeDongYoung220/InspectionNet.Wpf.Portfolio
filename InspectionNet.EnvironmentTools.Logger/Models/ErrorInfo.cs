using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Models;

namespace InspectionNet.EnvironmentTools.Logger.Models
{
    public class ErrorInfo : IErrorInfo
    {
        public ViErrorCode? Code { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
