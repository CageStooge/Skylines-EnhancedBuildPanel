﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            string logTime = Debug.LogTime();
            var message = string.Format("{0} {1} {2}", Prefix, logTime, str);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, message);
            UE.Debug.Log(message);
        }

        public static void LogWarning(string str)
        {
            string logTime = Debug.LogTime();
            var message = string.Format("{0} {1} {2}", Prefix, logTime, str);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, message);
            UE.Debug.LogWarning(message);
        }

        public static void LogError(string str)
        {
            string logTime = Debug.LogTime();
            var message = string.Format("{0} {1} {2}", Prefix, logTime, str);
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, message);
            UE.Debug.LogError(message);
        }

        private static string LogTime()
        {
            string str = System.DateTime.Now.ToString("G");
            return str;
        }

        public static void dumpObject(object myObject)
        {
            string myObjectDetails = "";
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(myObject))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(myObject);
                myObjectDetails += name + ": " + value + "\n";
            }
            Debug.Log(myObjectDetails);
        }
    }
}
