using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedBuildPanel;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EnhancedBuildPanel.GUI_09
{
    public class UIScrollPanel : UIPanel
    {
        #region variables


        private bool resizing = false;
        private UITitleBar _title;
        private Vector2 _resizeHandle;
        private bool moving = false;

        #endregion

        #region Start Function

        public override void Start()
        {
            Debug.Log(String.Format("Startup setting values", ""));

            name = EnhancedBuildPanel.Acronym;
            atlas = UIUtils.GetAtlas("Ingame");
            backgroundSprite = "UnlockingPanel2";
            size = new Vector2(400f, 400f);
            pivot = UIPivotPoint.TopLeft;
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = 400f;
            height = 400f;
            m_MaxSize = new Vector2(600f, 600f);
            m_MinSize = new Vector2(300f, 300f);
            autoFitChildrenHorizontally = false;
            relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width)/2),
                Mathf.Floor((GetUIView().fixedHeight - height)/2));

            CreateTitleBar();
            CreateResizeHandle();
            GetAssetButtons();
            base.Start();


            #endregion
        }

        #region Setup Controls
        private void CreateTitleBar()
        {
            _title = AddUIComponent<UITitleBar>();
            _title.iconSprite = "IconAssetBuilding";
            _title.title = "Enhanced Build Panel" + EnhancedBuildPanel.Version;
        }

        private void CreateResizeHandle()
        {
            Debug.Log(String.Format("Setting up resize handle", ""));
            var resizeButton = this.AddUIComponent<UIButton>();
            resizeButton.name = EnhancedBuildPanel.Acronym + "_ResizeButton";
            resizeButton.size = new Vector2(24.0f, 24.0f);
            resizeButton.AlignTo(this, UIAlignAnchor.TopLeft);
            resizeButton.normalBgSprite = "buttonresize";
            resizeButton.normalFgSprite = "buttonresize";
            resizeButton.hoveredBgSprite = "buttonresize";
            resizeButton.pressedFgSprite = "buttonresize";

            Debug.Log(String.Format("Setting up Mouse Hover event", ""));
            resizeButton.eventMouseHover += (component, param) =>
            {
                resizeButton.color = Color.red;
            };

            Debug.Log(String.Format("Setting up Mouse down event", ""));
            resizeButton.eventMouseDown += (component, param) =>
            {
                resizeButton.color = Color.black;
                resizing = true;
                _resizeHandle = Input.mousePosition;
            };

            Debug.Log(String.Format("Setting up mouse up event", ""));
            resizeButton.eventMouseUp += (component, param) =>
            {
                resizeButton.color = Color.white;
                resizing = false;
                _resizeHandle = Vector2.zero;
            };

            Debug.Log(String.Format("Setting resize button to new relative position", ""));
            resizeButton.relativePosition = new Vector3(0.0f, this.size.y, 0.0f);

            

        }

        private void GetAssetButtons()
        {
            
            IButtons buttons = new GetPanelButtons();
            Debug.Log(String.Format("Setting up icons", ""));
            buttons.UIButtons("RoadsSmallPanel", this);
            
            float x = 0.0f;
            float y = 0.0f;
            float iconWidth = this.transform.GetChild(0).GetComponent<UIButton>().size.x;
            float iconHeight = this.transform.GetChild(0).GetComponent < UIButton>().size.y;
            Debug.Log(String.Format("Looping through children", ""));
            for (int i = 0; i < this.transform.GetChildCount();i++)
            {
                /*
                 * What is being done is as each icon has their position placed into the panel during resizing,
                 * we track the width that the icons are using per row. When it reaches a point that it will 
                 * result in placing an icon outside of the panel, we shift down to the next row, and repeat the process
                 */
                var child = this.transform.GetChild(i).GetComponent<UIButton>();
                child.relativePosition = new Vector3(x, y, 0.0f);
                x += iconWidth;
                if (x >= this.width - iconWidth)
                {
                    Debug.Log(String.Format("Need to move icons to a new row", ""));
                    x = 0.0f;
                    y += iconHeight;

                }
            }
        }

        #endregion
    }
}

    
   



