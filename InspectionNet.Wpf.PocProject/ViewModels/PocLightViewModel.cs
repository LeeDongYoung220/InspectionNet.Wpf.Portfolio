using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InspectionNet.Core.Models;
using InspectionNet.Core.Services;

using InspectionNet.Wpf.Common.MainFrame.ViewModels;

namespace InspectionNet.Wpf.PocProject.ViewModels
{
    public class PocLightViewModel : ObservableObject, ILightViewModel
    {
        #region Variables
        private readonly ILightService _lightService;
        private IList<string> _portNames;
        private ILightController _lightController;
        private IList<ILightProtocol> _lightProtocols;
        #endregion

        #region Properties
        public IList<string> PortNames
        {
            get => _portNames;
            set => SetProperty(ref _portNames, value);
        }
        public IList<int> BaudRates { get; }
        public IList<int> DataBits { get; }
        public IList<Parity> Parities { get; }
        public IList<StopBits> StopBits { get; }
        public ILightController LightController
        {
            get => _lightController;
            set => SetProperty(ref _lightController, value);
        }
        public IList<ILightProtocol> LightProtocols
        {
            get => _lightProtocols;
            set => SetProperty(ref _lightProtocols, value);
        }
        public ICommand SearchPortCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand CloseCommand { get; }
        #endregion

        #region Events

        #endregion

        #region Constructor
        public PocLightViewModel(ILightService lightService)
        {
            _lightService = lightService;
            PortNames = _lightService.PortNames;
            BaudRates = _lightService.BaudRates;
            DataBits = _lightService.DataBits;
            Parities = _lightService.Parities;
            StopBits = _lightService.StopBits;

            _lightController = _lightService.GetLightController();

            _lightService.PortListChanged += LightService_PortListChanged;

            SearchPortCommand = new RelayCommand(SearchPort);
            OpenCommand = new RelayCommand(Open);
            CloseCommand = new RelayCommand(Close);
        }
        #endregion

        #region Finalizer
        public void DisposeLightService()
        {
            _lightService.PortListChanged -= LightService_PortListChanged;
        }
        #endregion

        #region Methods
        private void LightService_PortListChanged(object sender, IEnumerable<string> e) => PortNames = [.. e];
        private void SearchPort() => _lightService.SearchPort();
        private void Open() => LightController.Open();
        private void Close() => LightController.Close();
        #endregion
    }
}