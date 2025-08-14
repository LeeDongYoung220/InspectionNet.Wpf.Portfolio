using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;
using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.Services
{
    public interface IRuleToolService : IToolService
    {
        void ConvertPositionInFixture(int v1, int v2, out double outputX, out double outputY);
        void CreateFixture(double finalX1, double finalY1, double finalX2, double finalY2);
        ILpRuleToolGroup CreateToolGroup(string calibToolFilePath = "");
        void RunSequence(ILpImage e);
    }
}
