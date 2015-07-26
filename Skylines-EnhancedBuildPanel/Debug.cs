using System;
using ColossalFramework.Plugins;
using UE = UnityEngine;

namespace EnhancedBuildPanel
{
    public class Debug
    {
        public const bool Enabled = false;
        private const string Prefix = "<Enhanced Build Panel> - ";

        public static void Log(string str)
        {
            var logTime = LogTime();
            var message = string.Format("{0} {1} {2}", Prefix, logTime, str);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, message);
            UE.Debug.Log(message);
        }

        public static void LogWarning(string str)
        {
            var logTime = LogTime();
            var message = string.Format("{0} {1} {2}", Prefix, logTime, str);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, message);
            UE.Debug.LogWarning(message);
        }

        public static void LogError(string str)
        {
            var logTime = LogTime();
            var message = string.Format("{0} {1} {2}", Prefix, logTime, str);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, message);
            UE.Debug.LogError(message);
        }

        private static string LogTime()
        {
            var str = DateTime.Now.ToString("G");
            return str;
        }
    }
}