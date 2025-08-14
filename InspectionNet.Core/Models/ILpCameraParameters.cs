using System.ComponentModel;

namespace InspectionNet.Core.Models
{
    public interface ILpCameraParameters : ILpCameraConfigParameters, INotifyPropertyChanged
    {
        IEnumerable<string> PixelFormatAllValues { get; }
        IEnumerable<string> TriggerModeAllValues { get; }
        IEnumerable<string> TriggerSourceAllValues { get; }
        IEnumerable<string> UserSetSelectorAllValues { get; }
        IEnumerable<string> UserSetDefaultAllValues { get; }

        event EventHandler UserSetLoaded;
        event EventHandler UserSetSaved;

        void TriggerSoftware();
        void UserSetLoad();
        void UserSetSave();

        IList<IGenApiParameter> ToList();
    }
}
