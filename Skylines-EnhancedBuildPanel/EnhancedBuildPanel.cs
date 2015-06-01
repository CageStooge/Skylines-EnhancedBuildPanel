using System;
using ColossalFramework.UI;
using UnityEngine;

namespace EnhancedBuildPanel
{


    public class EnhancedBuildPanel : MonoBehaviour
    {

        private static readonly string configPath = "EnhancedBuildPanel.xml";
        private Configuration config;

        // Attempt to read the XML file to determine the last settings of the window
        // If no file is found, then load the default settings and use those.
        private void LoadConfig()
        {
            config = Configuration.Deserialize(configPath);

            // We'll check to make sure all the variable's were set, and that the mouse scroll wheel speed has a value of at least 1. 
            // This means, that in conjunction with the force refresh, a user should be able to change scroll speed without having to 
            // restart the game.
            if (config.panelPosition == null | config.panelSize == null || config.mouseScrollSpeed <= 0) 
            {
                Debug.Log("No configuration file detected. Creating a fresh one");
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

        private UIPanel[] panels;

        private static readonly string[] panelNames = new string[]
		{
			"RoadsSmallPanel",
			"RoadsMediumPanel",
			"RoadsLargePanel",
			"RoadsHighwayPanel",
			"RoadsIntersectionPanel",
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
			"BeautificationPropsPanel"
			/*"MonumentCategory1Panel",
            "MonumentCategory2Panel",
            "MonumentCategory3Panel",
            "MonumentCategory4Panel",
            "MonumentCategory5Panel",
            "MonumentCategory6Panel",*/
			//"WondersDefaultPanel"
		};

        void UpdatePanel(UIPanel panel)
        {
            var tabContainer = panel.gameObject.transform.parent.GetComponent<UITabContainer>();
            if (!config.panelPositionSet)
            {
                Debug.Log("Resetting panel to new size");
                config.panelPosition = tabContainer.relativePosition;
                config.panelSize = tabContainer.size;
                config.panelPositionSet = true;
                config.mouseScrollSpeed = 128.0f;
                Debug.Log(string.Format("Panel reset to position {0} and size {1}", config.panelPosition, config.panelSize));
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

            var groupToolStrip = tabContainer.transform.parent.GetComponent<UIPanel>()
                .Find<UITabstrip>("GroupToolstrip");
            if (groupToolStrip != null)
            {
                groupToolStrip.AlignTo(panel, UIAlignAnchor.TopLeft);
                groupToolStrip.relativePosition = new Vector3(8.0f, -20.0f, 0.0f);
                groupToolStrip.zOrder = -9999;
            }

            var scrollBar = panel.Find<UIScrollbar>("Scrollbar");
            try
            {
                
            scrollBar.autoHide = false;
            scrollBar.size = new Vector2(20.0f, tabContainer.size.y - 26.0f);
            scrollBar.orientation = UIOrientation.Vertical;
            scrollBar.isInteractive = true;
            scrollBar.isVisible = true;
            scrollBar.enabled = true;
            scrollBar.relativePosition = new Vector3(tabContainer.size.x - 20.0f - 2.0f, 0.0f, 0);
            scrollBar.incrementAmount = 10;
                scrollBar.Find<UIButton>("ArrowLeft").isVisible = false;
                scrollBar.Find<UIButton>("ArrowRight").isVisible = false;
            }
            catch (Exception ex)
            {
                Debug.Log(String.Format("Exception occured during Scrollbar setup : ", ex));
            }

            var trackSprite = scrollBar.Find<UISlicedSprite>("Track");
            UISlicedSprite thumbSprite = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                Debug.Log(string.Format("An exception occured while creating scroll bar : {0}", ex));
            }

            trackSprite.size = scrollBar.size;
            thumbSprite.width = trackSprite.width;

            var resizeButton = scrollBar.Find<UIButton>("ResizeButton");
            if (resizeButton == null)
            {
                try
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
                        SaveConfig();
                    };
                }
                catch (Exception ex)
                {
                    Debug.Log(string.Format("An exception occured when creating resize button : {0}", ex));
                }
            }

            resizeButton.relativePosition = new Vector3(0.0f, scrollBar.size.y, 0.0f);

            if (scrollablePanel.name != "ImprovedScrollablePanel")
            {
                scrollablePanel.name = "ImprovedScrollablePanel";
                scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
                scrollablePanel.horizontalScrollbar = null;
                scrollablePanel.verticalScrollbar = scrollBar;
                scrollablePanel.scrollWheelAmount = 64;
                scrollablePanel.autoLayout = false;
                scrollablePanel.autoSize = false;
                scrollablePanel.relativePosition = new Vector3(2.0f, 2.0f, 0.0f);

                scrollablePanel.eventMouseWheel += (component, param) =>
                {
                    scrollablePanel.scrollPosition = new Vector2(0.0f, scrollablePanel.scrollPosition.y + -param.wheelDelta * config.mouseScrollSpeed);
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

            scrollablePanel.size = new Vector2(tabContainer.size.x - 32.0f, config.panelSize.y - 2.0f);

            if (itemCount == 0)
            {
                return;
            }

            float x = 0.0f;
            float y = 0.0f;
            float width = scrollablePanel.transform.GetChild(0).GetComponent<UIButton>().size.x;
            float height = scrollablePanel.transform.GetChild(0).GetComponent<UIButton>().size.y;

            try
            {
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
            catch(Exception ex)
            {
                Debug.Log(string.Format("An exception occured when building Asset list : {0}", ex));
            }
            
        }

        private UIPanel openPanel = null;

        void Start()
        {
            LoadConfig();

            panels = new UIPanel[panelNames.Length];
            for (int i = 0; i < panelNames.Length; i++)
            {
                try
                {
                    panels[i] = GameObject.Find(panelNames[i]).GetComponent<UIPanel>();
                }
                catch (Exception)
                {
                    Debug.Log(String.Format("Couldn't find panel with name {0}", panelNames[i]));
                }
            }
        }

        void Update()
        {
            try
            {

                if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.P)) // Let's the user force a refresh in case the panel gets stuck somehow.
                {
                    //  Reset panel size to 600 wid x 110 tall, and moves it back to the original anchor point position.
                    Debug.Log("User requested a forced refresh");
                    config.panelSize = new Vector2(600.0f, 110f);
                    config.panelPosition = new Vector2(0.0f,0.0f);
                    SaveConfig();
                    openPanel = null;
                }

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
                    foreach (var panel in panels)
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
            catch (Exception ex)
            {
                Debug.Log(String.Format("EXCEPTION: {0}", ex));

            }
        }

    }

}
