using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedBuildPanel.GUI_09;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

using Object = UnityEngine.Object;


namespace EnhancedBuildPanel 
{
    public interface IButtons
    {
        void UIButtons(string panelName, UIComponent component);

    }

    class GetPanelButtons : IButtons
    {
        /* 
         * parentPanel just lets us know what to attach the buttons to.
         */

        public void UIButtons(string panelName, UIComponent component)
        {
            if (panelName == null)
            {
                Debug.Log(String.Format("No panel was given", ""));
                return;
            }
            string _panelName = panelName;
            Debug.Log(String.Format("Checking to see if we can find {0}", panelName));
            var panels = GameObject.Find(panelName);
            
            //var buttonList = new List<UIButton>();
            var scrollablePanel = panels.GetComponentInChildren<UIScrollablePanel>();
            
            Debug.Log("get components in scroll panel");
            if (scrollablePanel == null)
            {
                Debug.Log(string.Format("GetPanelButtons did not find a scrollable panel for panel : {0}", panelName));
                return;
            }
            
            Debug.Log("Getting the buttons together.");
            var panelButtons = scrollablePanel.GetComponentsInChildren<UIButton>(true);
            
            Debug.Log("Checking if the buttons just tried to get are null");
            if (panelButtons == null)
            {
                Debug.Log(string.Format("GetPanelButtons did not find any buttons to use in panel : {0}", panelName));
                return;
            }

            foreach (var button in panelButtons)
            {
                try
                {
                    Debug.Log(string.Format("creating clone of {0}", button));
                    UIButton cloneButton = new UIButton();
                    cloneButton = button;
                    Debug.Log(string.Format("Setting fields for cloned object"));
                    
                    cloneButton.name = EnhancedBuildPanel.Acronym + "_" + button.name;
                    Debug.Log(string.Format("attaching event onButtonclicked to button {0}", cloneButton));
                   
                    cloneButton.eventClick += setTool;
                    Debug.Log("Adding to Panel");


                    Debug.Log(string.Format("clonebutton is {0}", cloneButton));
                    cloneButton.transform.parent = component.transform;

                    Debug.Log(string.Format("Cloned button {0} to button {1}  and attached it to {2}!", button, cloneButton, cloneButton.parent));
                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("Exception cloning buttons : {0}", e));
                }
            }
        }

        private void setTool(UIComponent component, UIMouseEventParameter eventParam)
        {


            string oldName = component.name.Remove(0, (EnhancedBuildPanel.Acronym.Length + 1));
            string _buttonName = oldName;
            Debug.Log(string.Format("_buttonName is {0}", _buttonName));
            switchToButtonByName(_buttonName);
        }
        private void switchToButtonByName(string name)
        {
            NetTool netTool = ToolsModifierControl.SetTool<NetTool>();
            Debug.Log("Checking if netTool is null");
            if (netTool != null)
                Debug.Log("netTool is not null");
            {
                foreach (NetCollection collection in NetCollection.FindObjectsOfType<NetCollection>())
                {
                    foreach (NetInfo prefab in collection.m_prefabs)
                    {
                        if (prefab.name == name)
                        {
                            Debug.Log(string.Format("prefab named {0} was found", prefab.name));
                            netTool.m_prefab = prefab;
                            return;
                        }
                    }
                }
            }
        }
    }
}

