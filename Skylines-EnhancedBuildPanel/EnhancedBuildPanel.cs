using System;
using ColossalFramework.IO;
using ColossalFramework.UI;
using UnityEngine;

namespace EnhancedBuildPanel
{

    public class ImprovedBuildPanel : MonoBehaviour
    {

        private static readonly string configPath = DataLocation.modsPath + "EnhancedBuildConfig.xml"; // Save to users local mod directory
        private Configuration config;
        private void LoadConfig()
        {
            config = Configuration.Deserialize(configPath);
            if (config == null)
            {
                config = new Configuration();
                SaveConfig();
            }
        }

        private void SaveConfig()
        {
            Configuration.Serialize(configPath, config);
        }

        private bool resizing = false;
        private Vector2 resizeHandle = Vector2.zero;
        private bool moving = false;
        private Vector2 moveHandle = Vector2.zero;

        #region Panel Names
        private UIPanel[] _panels;
        private static readonly string[] PanelNames = new string[]
		{
			"RoadsSmallPanel",
			"RoadsMediumPanel",
			"RoadsLargePanel",
			"RoadsHighwayPanel",
			"RoadsIntersectionPanel",
            "Some RoadsPanel",
			//"ZoningDefaultPanel",
			//"DistrictDefaultPanel",
			"ElectricityDefaultPanel",
			"WaterAndSewageDefaultPanel",
			"GarbageDefaultPanel",
			"HealthcareDefaultPanel",
			"FireDepartmentDefaultPanel",
			"PoliceDefaultPanel",
			"EducationDefaultPanel",
			"PublicTransportBusPanel",
			"PublicTransportMetroPanel",
			"PublicTransportTrainPanel",
			"PublicTransportShipPanel",
			"PublicTransportPlanePanel",
			"BeautificationParksnPlazasPanel",
			"BeautificationPathsPanel",
			"BeautificationPropsPanel",
            "MonumentLandmarksPanel",
			"MonumentCategory1Panel",
            "MonumentCategory2Panel",
            "MonumentCategory3Panel",
            "MonumentCategory4Panel",
            "MonumentCategory5Panel",
            "MonumentCategory6Panel"
			//"WondersDefaultPanel"
		};
        #endregion
        #region UpdatePanel
        void UpdatePanel(UIPanel panel)
        {
            #region TabContainer
            var tabContainer = panel.gameObject.transform.parent.GetComponent<UITabContainer>();

       /*     var dragHandle = tabContainer.Find<UIDragHandle>("DragHandler");
            if (dragHandle == null)
            {
                dragHandle = tabContainer.AddUIComponent<UIDragHandle>();
                dragHandle.name = "DragHandler";
                            dragHandle.autoSize = false;
            dragHandle.size = new Vector2(24.0f, 24.0f);
            dragHandle.relativePosition = new Vector3(tabContainer.size.x - 24.0f, 24.0f, 0.0f);
            dragHandle.isInteractive = true;
            dragHandle.isVisible = true;
            dragHandle.enabled = true;
            dragHandle.color = Color.white;

            }
        */

            if (!config.panelPositionSet)
            {
                config.panelPosition = tabContainer.relativePosition;
                config.panelSize = tabContainer.size;
                config.panelPositionSet = true;
            }

            var scrollablePanel = panel.Find<UIScrollablePanel>("ScrollablePanel");
            var itemCount = scrollablePanel.transform.childCount;

            tabContainer.relativePosition = config.panelPosition;
            tabContainer.size = config.panelSize;

            if (tabContainer.absolutePosition.x + tabContainer.size.x >= Screen.width)
            {
                tabContainer.absolutePosition = new Vector2(Screen.width - tabContainer.size.x, tabContainer.absolutePosition.y);
            }

            if (tabContainer.absolutePosition.y + tabContainer.size.y >= Screen.height - 120.0f)
            {
                tabContainer.absolutePosition = new Vector2(tabContainer.absolutePosition.x, (Screen.height - 120.0f) - tabContainer.size.y);
            }

            if (tabContainer.absolutePosition.x <= 0.0f)
            {
                tabContainer.absolutePosition = new Vector2(0.0f, tabContainer.absolutePosition.y);
            }

            if (tabContainer.absolutePosition.y <= 0.0f)
            {
                tabContainer.absolutePosition = new Vector2(tabContainer.absolutePosition.x, 0.0f);
            }
            #endregion

            #region ToolStrip
            var groupToolStrip = tabContainer.transform.parent.GetComponent<UIPanel>()
                .Find<UITabstrip>("GroupToolstrip");
            if (groupToolStrip != null)
            {
                groupToolStrip.AlignTo(panel, UIAlignAnchor.TopLeft);
                groupToolStrip.relativePosition = new Vector3(8.0f, -20.0f, 0.0f);
                groupToolStrip.zOrder = -9999;
            }
            #endregion

            #region Scrollbar
            var scrollBar = panel.Find<UIScrollbar>("Scrollbar");
            scrollBar.autoHide = false;
            scrollBar.size = new Vector2(20.0f, tabContainer.size.y - 26.0f);
            scrollBar.orientation = UIOrientation.Vertical;
            scrollBar.isInteractive = true;
            scrollBar.isVisible = true;
            scrollBar.enabled = true;
            scrollBar.relativePosition = new Vector3(tabContainer.size.x - 20.0f,-2.0f,0.0f);

            // ******* This is what determines the scrolling speed. We will set this in the XML file, but in case it's not //
            // ******* there, we will set it to a default.
            if (config.scrollSpeed <= 10)
            {
                config.scrollSpeed = 160;
             }
            scrollBar.incrementAmount = config.scrollSpeed;

            try
            {
                scrollBar.Find<UIButton>("ArrowLeft").isVisible = false;
                scrollBar.Find<UIButton>("ArrowRight").isVisible = false;
            }
            catch (Exception ex)
            {
                Debug.Log(String.Format("Exception: {0}", ex));

            }
            #endregion

            #region Scrollbar track bar
            var trackSprite = scrollBar.Find<UISlicedSprite>("Track");
            UISlicedSprite thumbSprite = null;

            if (trackSprite == null)
            {
                trackSprite = scrollBar.AddUIComponent<UISlicedSprite>();
                trackSprite.name = "Track";
                trackSprite.relativePosition = Vector2.zero;
                trackSprite.autoSize = true;
                trackSprite.fillDirection = UIFillDirection.Horizontal;
                trackSprite.spriteName = "ScrollbarTrack";
                scrollBar.trackObject = trackSprite;
                thumbSprite = trackSprite.AddUIComponent<UISlicedSprite>();
                thumbSprite.name = "Thumb";
                thumbSprite.relativePosition = Vector2.zero;
                thumbSprite.fillDirection = UIFillDirection.Horizontal;
                thumbSprite.autoSize = true;
                thumbSprite.spriteName = "ScrollbarThumb";
                scrollBar.thumbObject = thumbSprite;
            }
            else
            {
                thumbSprite = trackSprite.Find<UISlicedSprite>("Thumb");
            }

            trackSprite.size = scrollBar.size;
            thumbSprite.width = trackSprite.width;
            #endregion

            #region Resize Button
            var resizeButton = scrollBar.Find<UIButton>("ResizeButton");
            if (resizeButton == null)
            {
                resizeButton = scrollBar.AddUIComponent<UIButton>();
                resizeButton.name = "ResizeButton";
                resizeButton.size = new Vector2(24.0f, 24.0f);
                resizeButton.AlignTo(scrollBar, UIAlignAnchor.TopLeft);
                resizeButton.normalFgSprite = "buttonresize";
                resizeButton.focusedFgSprite = "buttonresize";
                resizeButton.hoveredFgSprite = "buttonresize";
                resizeButton.pressedFgSprite = "buttonresize";
                resizeButton.disabledFgSprite = "buttonresize";

                resizeButton.eventMouseHover += (component, param) =>
                {
                    resizeButton.color = Color.grey;
                };

                resizeButton.eventMouseDown += (component, param) =>
                {
                    resizeButton.color = Color.black;
                    resizing = true;
                    resizeHandle = Input.mousePosition;
                };

                resizeButton.eventMouseUp += (component, param) =>
                {
                    resizeButton.color = Color.white;
                    resizing = false;
                    resizeHandle = Vector2.zero;
                    tabContainer.size = config.panelSize;
                    SaveConfig();
                };
            }

            resizeButton.relativePosition = new Vector3(0.0f, scrollBar.size.y, 0.0f);
            #endregion

            #region Scrollable Panel
            if (scrollablePanel.name != "ImprovedScrollablePanel")
            {
                scrollablePanel.name = "ImprovedScrollablePanel";
                scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                scrollablePanel.horizontalScrollbar = null;
                scrollablePanel.verticalScrollbar = scrollBar;
                scrollablePanel.scrollWheelAmount = 32;
                scrollablePanel.autoLayout = false;
                scrollablePanel.autoSize = false;
                scrollablePanel.relativePosition = new Vector3(2.0f, 2.0f, 0.0f);

                scrollablePanel.eventMouseWheel += (component, param) =>
                {
                    scrollablePanel.scrollPosition = new Vector2(0.0f, scrollablePanel.scrollPosition.y + -param.wheelDelta * 32.0f);
                };

                scrollBar.eventValueChanged += delegate(UIComponent component, float value)
                {
                    scrollablePanel.scrollPosition = new Vector2(0.0f, value);
                };
                
                scrollablePanel.eventMouseDown += (component, param) =>
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        moving = true;
                        moveHandle = Input.mousePosition;
                    }
                };

                scrollablePanel.eventMouseUp += (component, param) =>
                {
                    moving = false;
                    moveHandle = Vector2.zero;
                    SaveConfig();
                };
            }

            scrollablePanel.size = new Vector2(tabContainer.size.x - 32.0f, tabContainer.size.y - 2.0f);

            if (itemCount == 0)
            {
                return;
            }

            float x = 0.0f;
            float y = 0.0f;
            float width = scrollablePanel.transform.GetChild(0).GetComponent<UIButton>().size.x;
            float height = scrollablePanel.transform.GetChild(0).GetComponent<UIButton>().size.y;

            for (int i = 0; i < scrollablePanel.transform.childCount; i++)
            {
                var child = scrollablePanel.transform.GetChild(i).GetComponent<UIButton>();
                child.relativePosition = new Vector3(x, y, 0.0f);
                x += width;

                if (x >= scrollablePanel.width - width)
                {
                    x = 0.0f;
                    y += height;
                }
            }
        }
            #endregion
        #endregion

        private void CreateDragHandle(UITabContainer parent)
        {
            var dragHandleObject = new GameObject("DragHandler");
            dragHandleObject.transform.parent = parent.transform;
            dragHandleObject.transform.localPosition = Vector3.zero;
            var dragHandle = dragHandleObject.AddComponent<UIDragHandle>();
            dragHandle.autoSize = false;
            dragHandle.width = 24.0f;
            dragHandle.height = 24.0f;
            dragHandle.zOrder = 0;
            dragHandle.BringToFront();
        }

        #region InitPanels
        private UIPanel openPanel = null;

   
        private void initPanels()
        {
            LoadConfig();

            _panels = new UIPanel[PanelNames.Length];
            int p = 0;
            for (int i = 0; i < PanelNames.Length; i++)
            {
                try
                {
                    Debug.Log(string.Format("Searching for panel {0}", PanelNames[i]));
                    if (GameObject.Find(PanelNames[i]).GetComponent<UIPanel>() != null)
                    {
                        Debug.Log(string.Format("Loading panel {0} into monitoring queue at position {1}", PanelNames[i],
                            p));
                        _panels[p] = GameObject.Find(PanelNames[i]).GetComponent<UIPanel>();
                        p++;
                    }
                    else
                    {
                        Debug.Log(string.Format("Unable to locate panel {0} in the UI, skipping ...", PanelNames[i]));
                    }
                }
                catch (Exception)
                {
                    return; // Just tossing this. I can't figure out the error. So screw it.
                }
            }
        }
        #endregion
        void Start()
        {
            initPanels();

        }
        
        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
            {
                initPanels();
            }
            try
            {
                if (openPanel != null)
                {
                    if (!openPanel.isVisible)
                    {
                        openPanel = null;
                    }
                }

                if (resizing)
                {
                    Vector2 pos = Input.mousePosition;
                    var delta = pos - resizeHandle;
                    resizeHandle = pos;
                    config.panelSize += new Vector2(delta.x, -delta.y);

                    if (config.panelSize.x <= 64.0f)
                    {
                        config.panelSize = new Vector2(64.0f, config.panelSize.y);
                    }

                    if (config.panelSize.y <= 64.0f)
                    {
                        config.panelSize = new Vector2(config.panelSize.x, 64.0f);
                    }

                    openPanel = null;
                }
                else if (moving)
                {
                    Vector2 pos = Input.mousePosition;
                    var delta = pos - moveHandle;
                    moveHandle = pos;
                    config.panelPosition += new Vector2(delta.x, -delta.y);

                    openPanel = null;
                }

                if (openPanel == null)
                {
                    foreach (var panel in _panels)
                    {
                        if (panel.isVisible)
                        {
                            openPanel = panel;
                            break;
                        }
                    }

                    if (openPanel != null)
                    {
                        UpdatePanel(openPanel);
                    }
                }
            }
            catch (Exception)
            {
                return; // Yes, I tossed the errors. I've spent well over 30 hours total trying to figure out the exception. Now I don't care. Tossing it away.
            }
        }


    }

}
