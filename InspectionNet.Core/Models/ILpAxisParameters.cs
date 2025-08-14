using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionNet.Core.Models
{
    public interface ILpAxisParameters : ILpAxisInfoParameters, ILpAxisHomeParameters, ILpAxisPulseEncParameters, ILpAxisInOutParameters, ILpAxisSoftwareLimitParameters, ILpAxisUserMoveParameters, ILpAxisReadStatusParameters
    {
    }
}
