using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using System;

namespace EnhancedBuildPanel
{

    public class Configuration
    {
        public Vector2 panelPosition;
        public Vector2 panelSize;
        public bool panelPositionSet = false;
        public float mouseScrollSpeed;

        public void OnPreSerialize()
        {
        }

        public void OnPostDeserialize()
        {
        }

        public static void Serialize(string filename, Configuration config)
        {
            var serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                using (var writer = new StreamWriter(filename))
                {
                    config.OnPreSerialize();
                    serializer.Serialize(writer, config);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("An exception occured writing XML file, exception {0}", ex));
            }

        }

        public static Configuration Deserialize(string filename)
        {
            var serializer = new XmlSerializer(typeof(Configuration));

            try
            {
                using (var reader = new StreamReader(filename))
                {
                    var config = (Configuration)serializer.Deserialize(reader);
                    config.OnPostDeserialize();
                    return config;
                }
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("An exception occured while attempting to read XML configuraiotn file : {0}", ex));
            }

            return null;
        }
    }

}