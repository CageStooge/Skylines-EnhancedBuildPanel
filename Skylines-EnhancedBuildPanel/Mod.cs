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
            Debug.Log("v1.2 - Now Monitoring Panels ... ");
            var uiView = GameObject.FindObjectOfType<UIView>();
            improvedBuildPanel = uiView.gameObject.AddComponent<ImprovedBuildPanel>();
        }

        public override void OnLevelUnloading()
        {
            Debug.Log("Monitoring shutting down ... ");
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