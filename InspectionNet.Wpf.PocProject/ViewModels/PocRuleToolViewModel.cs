using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Mediators;
using InspectionNet.Core.Models;
using InspectionNet.Core.Services;
using InspectionNet.Core.VisionTools;
using InspectionNet.Wpf.Common.MainFrame.ViewModels;
using InspectionNet.Wpf.Common.Models;
using InspectionNet.Wpf.PocProject.Models;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocRuleToolViewModel : ObservableObject, IRuleToolViewModel
    {
        #region Variables
        private const string _toolBlockTitle = "Tool Block Edit";
        private const string _calibToolTitle = "Checkerboard Calibration Tool Edit";
        private readonly IRuleToolService _ruleToolService;
        private readonly IDisplayService _displayService;
        private IWpfToolGroup _currentToolGroup;
        private IToolGroupParameter currentToolGroupParameters;
        #endregion

        #region Properties
        public string CameraId { get; }
        public ICommand OpenCalibToolCommand { get; private set; }
        public ICommand OpenToolBlockCommand { get; private set; }
        public ICommand RuleRunCommand { get; private set; }
        public IToolGroupParameter CurrentToolGroupParameters { get => currentToolGroupParameters; private set => SetProperty(ref currentToolGroupParameters, value); }
        public IWpfToolGroup CurrentToolGroup
        {
            get => _currentToolGroup;
            set
            {
                if (SetProperty(ref _currentToolGroup, value))
                {
                    CurrentToolGroupParameters = new ToolGroupParameter(_currentToolGroup);
                }
            }
        }
        #endregion

        #region Events
        #endregion

        #region Constructor
        public PocRuleToolViewModel(IRuleToolService ruleToolService, IDisplayService displayService)
        {
            _ruleToolService = ruleToolService;
            InitRuleToolService();
            _displayService = displayService;
            InitCommand();
        }

        private void InitRuleToolService()
        {
            CurrentToolGroup = _ruleToolService.CurrentToolGroup as IWpfToolGroup;
            _ruleToolService.CurrentToolRan += RuleToolService_CurrentToolRan;
            _ruleToolService.CurrentToolGroupChanged += RuleToolService_CurrentToolGroupChanged;
        }

        private void RuleToolService_CurrentToolGroupChanged(object sender, ILpToolGroup e)
        {
            CurrentToolGroup = e as IWpfToolGroup;
        }

        private void RuleToolService_CurrentToolRan(object sender, ILpToolResult e)
        {
            _displayService.ToolViewDisplay.LaonResult = e;
        }

        private void InitCommand()
        {
            OpenCalibToolCommand = new RelayCommand(OpenCalibTool);
            OpenToolBlockCommand = new RelayCommand(OpenToolBlock);
            RuleRunCommand = new RelayCommand(RunRuleToolGroup);
        }
        #endregion

        #region Finalizer
        // No need for a finalizer in this case.
        #endregion

        #region Methods
        private void RunRuleToolGroup()
        {
            ILpImage currentImage = GetCurrentImage();
            CurrentToolGroup?.Run(currentImage);
        }

        private void OpenToolBlock()
        {
            var control = CurrentToolGroup?.ToolBlockEditControl;
            ShowControl(_toolBlockTitle, control);
        }

        private void OpenCalibTool()
        {
            var control = CurrentToolGroup?.CalibCheckerboardEditControl;
            ShowControl(_calibToolTitle, control);
        }

        private ILpImage GetCurrentImage()
        {
            return CurrentToolGroup?.InputImage;
        }

        private static void ShowControl(string title, Control control)
        {
            Window window = new()
            {
                Title = title,
                Content = control,
            };
            window.Show();
        }
        #endregion
    }
}
