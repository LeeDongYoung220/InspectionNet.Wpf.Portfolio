using System;
using System.Windows.Input;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Implementations
{
    public abstract class CommandParameter : ObservableObject, IGenApiParameter
    {
        public abstract string Name { get; }

        public abstract bool IsWritable { get; }

        public abstract ICommand ExecuteCommand { get; }

        public abstract event EventHandler Executed;

        public abstract void NotifyIsWritableChanged();
    }
}
