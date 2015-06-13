using System;
using UnityEngine;
using ICities;
using ColossalFramework.UI;

namespace EnhancedBuildPanel
{
    public class EnhancedBuildPanel : LoadingExtensionBase, IUserMod
    {
        public static readonly string Version = "2.0";
        private static UI.UIMainPanel _mainPanel;
        public static bool StopLoading = false;

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
            UIView view = UIView.GetAView();
            try
            {
                _mainPanel = (UI.UIMainPanel) view.AddUIComponent(typeof (UI.UIMainPanel));
            }
            catch (Exception e)
            {
                StopLoading = true;
                Debug.Log(string.Format("Could not create Enhanced Build UI Panel. Restart the game. \n Exception: {0}",
                    e));
                throw;
            }

        }

        public override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            if (_mainPanel == null)
                return;
            _mainPanel.RemoveUIComponent(_mainPanel);
            GameObject.Destroy(_mainPanel);
        }

        public override void OnReleased()
        {
            base.OnReleased();
            base.OnLevelUnloading();
            if (_mainPanel == null)
                return;
            _mainPanel.RemoveUIComponent(_mainPanel);
            GameObject.Destroy(_mainPanel);
        }
    }
}

