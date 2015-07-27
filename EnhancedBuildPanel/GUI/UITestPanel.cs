using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedBuildPanel;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EnhancedBuildPanel.GUI_01
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
            var resizeButton = this.AddUIComponent<UIButton>();
            resizeButton.name = EnhancedBuildPanel.Acronym + "_ResizeButton";
            resizeButton.size = new Vector2(24.0f, 24.0f);
            resizeButton.AlignTo(this, UIAlignAnchor.TopLeft);
            resizeButton.normalBgSprite = "buttonresize";
            resizeButton.normalFgSprite = "buttonresize";
            resizeButton.hoveredBgSprite = "buttonresize";
            resizeButton.pressedFgSprite = "buttonresize";

            resizeButton.eventMouseHover += (component, param) =>
            {
                resizeButton.color = Color.red;
            };

            resizeButton.eventMouseDown += (component, param) =>
            {
                resizeButton.color = Color.black;
                resizing = true;
                _resizeHandle = Input.mousePosition;
            };

            resizeButton.eventMouseUp += (component, param) =>
            {
                resizeButton.color = Color.white;
                resizing = false;
                _resizeHandle = Vector2.zero;
            };

            resizeButton.relativePosition = new Vector3(0.0f, this.size.y, 0.0f);


        }


        private void CreateScrollBar()
        {
            
        }
        #endregion

    }
}



