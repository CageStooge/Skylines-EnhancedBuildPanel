using System;
using ColossalFramework.UI;
using EnhancedBuildPanel.UI;
using ICities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EnhancedBuildPanel
{
    public class EnhancedBuildPanel2 : LoadingExtensionBase, IUserMod
    {
        private static GameObject gameObject;
        private static UIMainPanel mainPanel;
        public static readonly string Version = "2.0";

        public string Name
        {
            get { return string.Format("Enhanced Build Panel version {0}", Version); }
        }

        public string Description
        {
            get { return "Enhanced In Game Asset Panel"; }
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            // Is it an actual game ?
            //if (mode != LoadMode.LoadGame && mode != LoadMode.NewGame)
            //return;

            // Creating GUI

            try
            {
                var view = UIView.GetAView();
                gameObject = new GameObject("EHB");
                gameObject.transform.SetParent(view.transform);

                mainPanel = gameObject.AddComponent<UIMainPanel>();
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Could not create the UI. Try to relaunch the game. \n Error: {0}", e));
            }
        }

        public override void OnLevelUnloading()
        {
            try
            {
                if (gameObject == null) return;

                Object.Destroy(gameObject);
                gameObject = null;
            }
            catch (Exception e)
            {
                Debug.Log(string.Format("Exception: {0}", e));
            }
        }
    }
}