namespace InspectionNet.Core.Services
{
    public interface IFileDialogService
    {
        string ShowFolderBrowserDialog();
        string ShowOpenFileDialog();
    }
}
