using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Enums;

namespace InspectionNet.Core.Models
{
    public interface IErrorInfo
    {
        string? StackTrace { get; set; }
        DateTime Timestamp { get; set; }
        ViErrorCode? Code { get; set; }
    }
}
