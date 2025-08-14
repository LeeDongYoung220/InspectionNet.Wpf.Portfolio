using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Mediators
{
    public interface ISequence
    {
        bool IsToolRun { get; set; }

        event EventHandler<ILpToolResult> VisionToolGroupCompleted;

        void Run();
        void StartAlign();
        void Stop();
    }
}
