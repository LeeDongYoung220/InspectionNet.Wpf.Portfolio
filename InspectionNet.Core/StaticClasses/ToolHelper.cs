using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using InspectionNet.Core.VisionTools;

namespace InspectionNet.Core.StaticClasses
{
    public static class ToolHelper
    {
        public static double GetTerminalValueDouble(ILpToolResult e, string keyName)
        {
            if (e?.Outputs == null)
                throw new InvalidOperationException("Tool result or outputs is null.");

            if (!e.Outputs.TryGetValue(keyName, out var outputXValue))
                throw new InvalidOperationException("OutputX not found in tool result outputs.");

            if (outputXValue == null)
                throw new InvalidOperationException("OutputX value is null.");

            if (!double.TryParse(outputXValue.ToString(), out double result))
            {
                throw new InvalidOperationException($"OutputX value '{outputXValue}' is not a valid double.");
            }
            return result;
        }
    }
}
