using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InspectionNet.Core.Models
{
    public interface ILpGrabberManager : IDisposable
    {
        IList<ILpGrabber> Discover();
    }
}
