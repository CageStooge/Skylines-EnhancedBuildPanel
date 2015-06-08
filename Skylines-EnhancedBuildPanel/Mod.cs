using System;
using UnityEngine;
using ICities;
using ColossalFramework.UI;

namespace EnhancedBuildPanel
{
    public class Mod : IUserMod
    {
        public string Name { get { return "Enhanced Build Panel"; } }
        public string Description { get { return "Enhanced In Game Asset Panel"; } }
    }

    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            ThreadingExtension.instance.OnLevelLoad();
        }
        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            ThreadingExtension.instance.OnLevelUnload();
        }
    }
    
    public class ThreadingExtension : ThreadingExtensionBase
    {
        public static ThreadingExtension instance;
        ImprovedBuildPanel improvedBuildPanel;

        public override void OnCreated(IThreading threading)
        {
            base.OnCreated(threading);
            ThreadingExtension.instance = this;
            this.OnLevelLoad();

        }
        public override void OnReleased()
        {
            base.OnReleased();
        }

        public void OnLevelLoad()
        {
            InitPanel();
        }

        public void OnLevelUnload()
        {
            DestroyPanel(improvedBuildPanel);

        }

        void DestroyPanel(Component panel)
        {
            if (improvedBuildPanel != null)
            {
                Debug.Log("Destroying improvedBuildPanel!");

                GameObject.Destroy(panel);
            }
        }

        void InitPanel()
        {
            DestroyPanel(improvedBuildPanel);

            //the game caches (UI?) classes, so while developing init your UI here
            //alternatively use your own class, but rename it before each recompile

            //UIPanel or any UIComponent you want

            //uiView = GameObject.FindObjectOfType<UIView>();
            Debug.Log("Creating new panel!");
            improvedBuildPanel = GameObject.FindObjectOfType<UIView>().gameObject.AddComponent<ImprovedBuildPanel>();

            /*
            panel = UIView.GetAView().AddUIComponent(typeof(UIPanel)) as UIPanel;
            panel.backgroundSprite = "GenericPanel";
            panel.color = new Color32(255, 0, 0, 100);
            panel.width = 100;
            panel.height = 200;
             */
        }


    }
}