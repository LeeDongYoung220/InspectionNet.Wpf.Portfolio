using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;

namespace InspectionNet.Core.Services
{
    public interface IMotionService
    {
        bool IsRepeat { get; set; }
        IEnumerable<ILpAxis> AxisList { get; }
        IEnumerable<ILpAxisPosition> AlignPositions { get; }
        IEnumerable<ILpAxisPosition> RunPositions { get; }

        event EventHandler InitializeCompleted;
        event EventHandler MoveCompleted;
        event EventHandler<ILpAxisPosition> InitPositionAdded;
        event EventHandler<ILpAxisPosition> InitPositionRemoved;
        event EventHandler<ILpAxisPosition> RunPositionAdded;
        event EventHandler<ILpAxisPosition> RunPositionRemoved;
        event EventHandler RunPositionCleared;

        void Close();
        void Run(int step);
        void LoadConfigFile(string filePath);
        void SaveConfigFile();
        void Initialize();
        void MoveAlignPosition(int index);
        void SetAlignFinalPosition(int index);
        void MoveTarget(double targetX, double targetY, bool isRelative = false);
        void MoveStop();
        void AddInitPosition(string name);
        void RemoveInitPosition(string name);
        void AddRunPosition(string name);
        void AddRunPosition(string name, double targetX, double targetY);
        void RemoveRunPosition(string name);
        void ClearRunPosition();
    }
}
