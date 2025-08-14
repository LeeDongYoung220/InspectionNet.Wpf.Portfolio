using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InspectionNet.Core.Models
{
    public interface ILpAxisPosition
    {
        string Name { get; set; }
        double TargetX { get; set; }
        double TargetY { get; set; }
        double FinalX { get; }
        double FinalY { get; }
        
        ICommand MoveTargetCommand { get; }

        void SetFinal(double x, double y);
    }
}
