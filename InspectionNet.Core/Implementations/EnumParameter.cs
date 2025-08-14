using System;
using System.Collections.Generic;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Implementations
{
    public abstract class EnumParameter : ObservableObject, IGenApiParameter
    {
        public abstract string Name { get; }

        public abstract bool IsWritable { get; }

        public abstract IEnumerable<string> AllValues { get; }

        public abstract string Value { get; set; }

        public abstract void NotifyIsWritableChanged();
    }
}
