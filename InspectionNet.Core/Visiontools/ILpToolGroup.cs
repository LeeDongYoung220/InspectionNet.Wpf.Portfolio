using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;

namespace InspectionNet.Core.VisionTools
{
    public interface ILpToolGroup : IDisposable
    {
        bool IsSaveOriginImage { get; set; }
        bool IsSaveCsv { get; set; }
        ILpImage InputImage { get; set; }

        event EventHandler InitializeCompleted;
        event EventHandler<ILpToolResult> ToolBlockCompleted;
        
        void Initialize(string dataFilePath = "");
        void Run(ILpImage inputImage);
    }
}
