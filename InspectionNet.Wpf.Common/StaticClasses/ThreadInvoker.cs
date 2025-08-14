using System.Windows;
using System.Windows.Threading;

namespace InspectionNet.Wpf.Common.StaticClasses
{
    public static class ThreadInvoker
    {
        public static void DispatcherInvoke(Action action, bool bAsync = false)
        {
            Dispatcher dispatchObject = Application.Current?.Dispatcher ?? null;
            if (dispatchObject == null || dispatchObject.CheckAccess())
            {
                action();
            }
            else
            {
                if (bAsync) dispatchObject.BeginInvoke(action);
                else dispatchObject.Invoke(action);
            }
        }
    }
}
