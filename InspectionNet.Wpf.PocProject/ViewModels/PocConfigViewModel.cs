using System;
using System.ComponentModel;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Models;
using InspectionNet.Core.Services;

using InspectionNet.Wpf.Common.MainFrame.ViewModels;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocConfigViewModel : ObservableObject, IConfigViewModel, IDisposable
    {
        #region Variables
        private readonly System.Windows.Forms.FolderBrowserDialog _folderBrowserDialog = new();
        private bool _saveImage;
        private string _saveImagePath;
        #endregion

        #region Properties
        public IConfigParameters ConfigParameters { get; }
        public bool SaveImage
        {
            get => _saveImage;
            set
            {
                if (SetProperty(ref _saveImage, value))
                {
                    ConfigParameters.SaveOriginImage = value;
                }
            }
        }
        public string SaveImagePath
        {
            get => _saveImagePath;
            set
            {
                if (SetProperty(ref _saveImagePath, value))
                {
                    ConfigParameters.SaveImagePath = value;
                }
            }
        }
        public ICommand SearchFolderCommand { get; }
        public string ErrorMessage { get; }
        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocConfigViewModel(IConfigService configService)
        {
            ConfigParameters = configService.GetConfigParameters();
            ConfigParameters.PropertyChanged += ConfigParameters_PropertyChanged;

            SearchFolderCommand = new RelayCommand(SearchFolder);
        }
        #endregion

        #region Finalizer
        private bool disposedValue;


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state.
                }
                DisposeConfigParameters();
                disposedValue = true;
            }
        }
        ~PocConfigViewModel()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        public void DisposeConfigParameters()
        {
            ConfigParameters.PropertyChanged -= ConfigParameters_PropertyChanged;
        }
        #endregion

        #region Methods
        private void ConfigParameters_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is IConfigParameters configParameters)
            {
                switch (e.PropertyName)
                {
                    case nameof(configParameters.SaveOriginImage):
                        SaveImage = configParameters.SaveOriginImage;
                        break;
                    case nameof(configParameters.SaveImagePath):
                        SaveImagePath = configParameters.SaveImagePath;
                        break;
                }
            }
        }

        private void SearchFolder()
        {
            if (_folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ConfigParameters.SaveImagePath = _folderBrowserDialog.SelectedPath;
            }
        }
        #endregion
    }
}
