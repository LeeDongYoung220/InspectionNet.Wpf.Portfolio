using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.VisionTools
{
    public interface ILpToolBlock : IDisposable
    {
        event EventHandler<ILpToolResult> ToolBlockCompleted;
        event EventHandler DataFileLoaded;

        void Run(ILpImage inputImage);
        void LoadDataFile(string filePath);
    }
}
