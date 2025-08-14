using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Services;
using InspectionNet.Core.VisionTools;
using InspectionNet.Wpf.Common.MainFrame.ViewModels;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocAiToolViewModel : ObservableObject, IAiToolViewModel
    {
        #region Variables

        private readonly System.Windows.Forms.OpenFileDialog _openFileDialog = new();
        private readonly IAiToolService _aiToolService;
        private readonly IDisplayService _displayService;
        private ILpToolGroup _currentToolGroup;

        #endregion

        #region Properties
        public ICommand LoadTrainFileCommand { get; private set; }
        public ICommand AiRunCommand { get; private set; }

        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocAiToolViewModel(IAiToolService aiToolService, IDisplayService displayService)
        {
            _aiToolService = aiToolService;
            _displayService = displayService;
            InitAiTool();
            InitCommand();
        }

        private void InitAiTool()
        {
            _currentToolGroup = _aiToolService.CurrentToolGroup;
            _aiToolService.CurrentToolRan += AiToolService_CurrentToolRan;
            _aiToolService.CurrentToolGroupChanged += AiToolService_CurrentToolGroupChanged;
        }

        private void AiToolService_CurrentToolGroupChanged(object sender, ILpToolGroup e)
        {
            _currentToolGroup = e;
        }

        private void AiToolService_CurrentToolRan(object sender, ILpToolResult e)
        {
            _displayService.ToolViewDisplay.LaonResult = e;
        }

        private void InitCommand()
        {
            LoadTrainFileCommand = new RelayCommand(LoadTrainFile);
            AiRunCommand = new RelayCommand(AiRun);
        }

        #endregion

        #region Finalizer

        #endregion

        #region Methods
        private void LoadTrainFile()
        {
            if (_openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string trainFilePath = _openFileDialog.FileName;
                _currentToolGroup.Initialize(trainFilePath);
            }
        }

        private void AiRun()
        {
            var inputImage = _currentToolGroup?.InputImage;
            if (inputImage == null)
            {
                Debug.WriteLine("Input image is null. Please set an input image before running the AI tool group.");
                return;
            }
            _currentToolGroup.Run(inputImage);
        }
        #endregion
    }
}
