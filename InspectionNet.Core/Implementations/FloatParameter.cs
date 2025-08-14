using System;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Implementations
{
    public abstract class FloatParameter : ObservableObject, IGenApiParameter
    {
        public abstract string Name { get; }

        public abstract bool IsWritable { get; }

        public abstract double Value{ get; set; }

        public abstract void NotifyIsWritableChanged();
    }
}
