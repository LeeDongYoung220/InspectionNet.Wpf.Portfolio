using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.Enums;

namespace InspectionNet.Core.Managers
{
    public interface ILogManager
    {
        void LogError(object sender, ViErrorCode errorCode, string message, Exception? ex, [CallerMemberName] string memberName = "");
        void LogInfo(object sender, string message, [CallerMemberName] string memberName = "");
        void LogWarning(object sender, string message, [CallerMemberName] string memberName = "");
    }
}
