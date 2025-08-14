using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Mediators;
using InspectionNet.Core.Services;

using InspectionNet.Wpf.Common.Models;

namespace InspectionNet.Wpf.PocProject.Models
{
    public class PositionGenerator : ObservableObject, IPositionGenerator
    {
        private readonly IMotionService _motionService;


        private double _startX;
        public double StartX
        {
            get => _startX;
            set => SetProperty(ref _startX, value);
        }

        private double _startY;
        public double StartY
        {
            get => _startY;
            set => SetProperty(ref _startY, value);
        }

        private int _rows;
        public int Rows
        {
            get => _rows;
            set => SetProperty(ref _rows, value);
        }

        private int _columns;
        public int Columns
        {
            get => _columns;
            set => SetProperty(ref _columns, value);
        }

        private double _pitchX;
        public double PitchX
        {
            get => _pitchX;
            set => SetProperty(ref _pitchX, value);
        }

        private double _pitchY;
        public double PitchY
        {
            get => _pitchY;
            set => SetProperty(ref _pitchY, value);
        }

        private bool _reverseDirMode;
        public bool ReverseDirMode
        {
            get => _reverseDirMode;
            set => SetProperty(ref _reverseDirMode, value);
        }

        public ICommand RunCommand { get; }

        public PositionGenerator(IMotionService motionService)
        {
            _motionService = motionService;

            RunCommand = new RelayCommand(Run);
        }

        private void Run()
        {
            _motionService.ClearRunPosition();

            double originX;
            double originY = StartY;
            int dir;
            for (int r = 0; r < Rows; r++)
            {
                originX = StartX;
                dir = 1;
                if (ReverseDirMode && r % 2 == 1)
                {
                    originX = StartX + PitchX * (Columns - 1);
                    dir = -1;
                }
                for (int c = 0; c < Columns; c++)
                {
                    string name = $"{r}row, {c}col";
                    double targetX = originX + PitchX * c * dir;
                    double targetY = originY + PitchY * r;
                    _motionService.AddRunPosition(name, targetX, targetY);
                }
            }
        }
    }
}
