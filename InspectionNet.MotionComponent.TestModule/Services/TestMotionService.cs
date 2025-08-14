using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Models;
using InspectionNet.Core.Services;

namespace InspectionNet.MotionComponent.TestModule.Services
{
    public class TestMotionService : IMotionService
    {
        public bool IsRepeat { get; set; }
        public IEnumerable<ILpAxis> AxisList { get; }
        public IEnumerable<ILpAxisPosition> AlignPositions { get; }
        public IEnumerable<ILpAxisPosition> RunPositions { get; }

        public event EventHandler InitializeCompleted;
        public event EventHandler MoveCompleted;
        public event EventHandler<ILpAxisPosition> InitPositionAdded;
        public event EventHandler<ILpAxisPosition> InitPositionRemoved;
        public event EventHandler<ILpAxisPosition> RunPositionAdded;
        public event EventHandler<ILpAxisPosition> RunPositionRemoved;
        public event EventHandler RunPositionCleared;

        public void AddInitPosition(string name)
        {
            throw new NotImplementedException();
        }

        public void AddRunPosition(string name)
        {
            throw new NotImplementedException();
        }

        public void AddRunPosition(string name, double targetX, double targetY)
        {
            throw new NotImplementedException();
        }

        public void ClearRunPosition()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void LoadConfigFile(string filePath)
        {
            throw new NotImplementedException();
        }

        public void MoveAlignPosition(int index)
        {
            throw new NotImplementedException();
        }

        public void MoveStop()
        {
            throw new NotImplementedException();
        }

        public void MoveTarget(double targetX, double targetY, bool isRelative = false)
        {
            throw new NotImplementedException();
        }

        public void RemoveInitPosition(string name)
        {
            throw new NotImplementedException();
        }

        public void RemoveRunPosition(string name)
        {
            throw new NotImplementedException();
        }

        public void Run(int step)
        {
            throw new NotImplementedException();
        }

        public void SaveConfigFile()
        {
            throw new NotImplementedException();
        }

        public void SetAlignFinalPosition(int index)
        {
            throw new NotImplementedException();
        }
    }
}
