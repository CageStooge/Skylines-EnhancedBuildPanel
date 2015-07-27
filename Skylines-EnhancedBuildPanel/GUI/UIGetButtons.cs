/*using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Reflection;
using UnityEngine.Events;

namespace EnhancedBuildPanel.GUI6
{
    public class DelegateScript : MonoBehaviour
    {
        public delegate void buttonEventHandler(GameObject button);

        public static event buttonEventHandler onButtonClick;

    }
    
    public class UIGetButtons
    {
        private UIComponent _uiComponent;
        private UIButton cloneButton;
        public UIMainPanel _uiMainPanel;
       

        public UIGetButtons(UIMainPanel uiMainPanel)
        {
            _uiMainPanel = uiMainPanel;
        }

        public IList<UIButton> UIButtons(string PanelName)
        {
            Debug.Log(string.Format("PanelName that was sent was {0}", PanelName));
            var panelName = PanelName;
            var panels = GameObject.Find(panelName);
            var buttonList = new List<UIButton>();

            var scrollablePanel = panels.GetComponentInChildren<UIScrollablePanel>();
            if (scrollablePanel == null)
            {
                Debug.Log(string.Format("GetPanelButtons did not find a scrollable panel for panel : {0}", panelName));
                return buttonList;
            }

            var panelButtons = scrollablePanel.GetComponentsInChildren<UIButton>(true);
            if (panelButtons == null)
            {
                Debug.Log(string.Format("GetPanelButtons did not find any buttons to use in panel : {0}", panelName));
                return buttonList;
            }
            
            foreach (var button in panelButtons)
            {
                try
                {
                    Type _type;
                  _type = button.GetType();
                _uiComponent = button.GetComponent<UIComponent>();
                    _uiComponent = Object.Instantiate(button);
                    
                    
                    _uiMainPanel.AddUIComponent(typeof(Type _type);
                    _uiMainPanel.eventComponentAdded

                    
     var gameButtonName = _uiComponent.name;
                    var prefix = EnhancedBuildPanel.Acronym;
                    var newGameButton = prefix + gameButtonName;

                    Debug.Log(string.Format("Cloning GameButtonName {0} to gameButton {1}", gameButtonName,
                        newGameButton));
                    
                    cloneButton = Object.Instantiate(button);
                    cloneButton = _uiMainPanel.AddUIComponent<UIButton>();
                    cloneButton.name = newGameButton;
                    

                    cloneButton.eventClick += delegate(MouseEventHandler)
                    
                    button.eventMouseDown += delegate()
                    MyDelegate buttonClick = GetByName(button, "OnClick");
                    cloneButton.eventClick += button.get

                        
                    cloneButton.eventClick += buttonClick;

                     
                    
                    Debug.Log(string.Format("Button cloned to {0} and parent object was set to {1}", cloneButton.name,
                        cloneButton.parent));
                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("Exception : {0}", e));
                }
            }

            var isEmpty = !buttonList.Any();
            if (isEmpty)
            {
                Debug.Log(string.Format("GetPanelButtons returned an empty list of buttons for panel : {0}", panelName));
                return buttonList;
            }
            return buttonList;
        }
    }
}
*/