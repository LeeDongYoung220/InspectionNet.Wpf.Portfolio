using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Services
{
    public interface IAiToolService : IToolService
    {
        ILpToolGroup CreateToolGroup();
    }
}
