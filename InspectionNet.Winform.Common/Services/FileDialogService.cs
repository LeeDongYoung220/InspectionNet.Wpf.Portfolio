using InspectionNet.Core.Services;

namespace InspectionNet.Winform.Common.Services
{
    public class FileDialogService : IFileDialogService
    {
        public string ShowFolderBrowserDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
            }
            return string.Empty;
        }

        public string ShowOpenFileDialog()
        {
            using (var dialog = new OpenFileDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    return dialog.FileName;
                }
            }
            return string.Empty;
        }
    }
}
