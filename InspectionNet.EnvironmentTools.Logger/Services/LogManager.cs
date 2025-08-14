using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Enums;
using InspectionNet.Core.Managers;
using InspectionNet.Core.Models;
using InspectionNet.EnvironmentTools.Logger.Models;

using log4net;
using log4net.Config;

namespace InspectionNet.EnvironmentTools.Logger.Services
{
    public class LogManager : ILogManager
    {
        #region Variables

        private readonly ILog? log;

        #endregion

        #region Properties

        #endregion

        #region Events
        public event EventHandler<IErrorInfo>? ErrorOccurred;
        #endregion

        #region Constructor

        public LogManager()
        {
            var cfgFile = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config"));
            if (cfgFile.Exists)
            {
                XmlConfigurator.Configure(cfgFile);
                log = log4net.LogManager.GetLogger(typeof(LogManager));
            }
        }

        public void LogInfo(object sender, string message, [CallerMemberName] string memberName = "")
        {
            log?.InfoFormat("[{0}.{1}] {2}", sender.GetType().Name, memberName, message);
        }

        public void LogWarning(object sender, string message, [CallerMemberName] string memberName = "")
        {
            log?.WarnFormat("[{0}.{1}] {2}", sender.GetType().Name, memberName, message);
        }
        private void RaiseErrorEvent(ViErrorCode? message, Exception? ex)
        {
            ErrorOccurred?.Invoke(this, new ErrorInfo
            {
                Code = message,
                StackTrace = ex?.StackTrace,
                Timestamp = DateTime.Now
            });
        }

        public void LogError(object sender, ViErrorCode errorCode, string message, Exception? ex, [CallerMemberName] string memberName = "")
        {
            log?.ErrorFormat("[{0}.{1}] {2}", sender.GetType().Name, memberName, message, ex);
            RaiseErrorEvent(errorCode, ex);
        }
        #endregion
    }
}
