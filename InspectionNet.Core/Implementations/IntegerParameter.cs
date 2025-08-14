using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Implementations
{
    public abstract class IntegerParameter : ObservableObject, IGenApiParameter
    {
        public abstract string Name { get; }
        public abstract bool IsWritable { get; }
        public abstract int Value { get; set; }

        public abstract void NotifyIsWritableChanged();
    }
}
