using System;
using System.Collections.Generic;

namespace InspectionNet.Core.Models
{
    public interface ILpGrabber : IDisposable
    {
        IList<ILpCamera> Discover();
    }
}
