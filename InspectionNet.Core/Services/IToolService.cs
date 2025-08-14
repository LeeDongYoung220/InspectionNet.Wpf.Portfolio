using System;

using InspectionNet.Core.Implementations;
using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Services
{
    public interface IToolService
    {
        ILpToolGroup? CurrentToolGroup { get; }

        event EventHandler? InitializeCompleted;
        event EventHandler<ILpToolGroup>? CurrentToolGroupChanged;
        event EventHandler<ILpToolResult>? CurrentToolRan;
        void Initialize();
    }
}
