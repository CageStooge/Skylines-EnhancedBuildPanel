using ColossalFramework.Plugins;
using UnityEngine;

namespace EnhancedBuildPanel
{
    public class DebugUtils
    {
        private static readonly string modPrefix = EnhancedBuildPanel.Acronym;

        public static void Message(string message)
        {
            Log(message);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, modPrefix + message);
        }

        public static void Warning(string message)
        {
            Debug.Log(modPrefix + message);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, modPrefix + message);
        }

        public static void Log(string message)
        {
            Debug.Log(modPrefix + message);
        }
    }
}