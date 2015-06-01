using ColossalFramework.Plugins;
using UE = UnityEngine;

namespace EnhancedBuildPanel
{
    public class Debug
    {
        public static void Log(string LogInput)
        {
            string AppVersion = "<Enhanced Build Panel v 1.0.2> : ";
            string LogDateTime = System.DateTime.Now.ToString("G");
            string LogOutput = AppVersion + LogDateTime + LogInput;
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, LogOutput);
            UE.Debug.Log(LogOutput);
        }
    }
}
