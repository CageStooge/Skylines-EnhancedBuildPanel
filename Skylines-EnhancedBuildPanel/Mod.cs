using ColossalFramework.UI;
using ICities;
using UnityEngine;

namespace EnhancedBuildPanel
{

    public class Mod : IUserMod
    {

        public string Name
        {
            get
            {
                return "Enhanced Build Panel";
            }
        }

        public string Description
        {
            get { return "Enhanced In Game Asset Panel"; }
        }

    }

    public class ModLoad : LoadingExtensionBase
    {

        private ImprovedBuildPanel improvedBuildPanel;

        public override void OnLevelLoaded(LoadMode mode)
        {

            Debug.Log("v1.2.0 - Defining Panel ... ");
            var uiView = GameObject.FindObjectOfType<UIView>();
            var oldPanel = uiView.gameObject.GetComponent<ImprovedBuildPanel>();
            if (oldPanel)
            {
                Debug.Log(string.Format("ImprovedBuildPanel is detected at game load, destroying old paenl", oldPanel));
                GameObject.DestroyObject(oldPanel);
            }
            improvedBuildPanel = uiView.gameObject.AddComponent<ImprovedBuildPanel>();
            Debug.Log("v1.3.1 Now monitoring panels");
        }

        public override void OnLevelUnloading()
        {
            Debug.Log("Monitoring shutting down ... ");
            Debug.Log("Removing Panel");
            GameObject.Destroy(improvedBuildPanel);
            GameObject.DestroyObject(improvedBuildPanel);
            if (improvedBuildPanel == null)
            {
                Debug.Log("improvedBuildPanel was destroyed");
            }
            else
            {
                Debug.Log("improvedBuildPanel was NOT destoryed");
            }
        }
    }

}