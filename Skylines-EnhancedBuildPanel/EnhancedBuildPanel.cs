﻿using System;
using ColossalFramework.UI;
using UnityEngine;

namespace EnhancedBuildPanel
{

    public class ImprovedBuildPanel : MonoBehaviour
    {

        private static readonly string ConfigPath = "EnhacnedBuildPanelConfig.xml";
        private Configuration _config;

        private void LoadConfig()
        {
            _config = Configuration.Deserialize(ConfigPath);
            if (_config == null)
            {
                _config = new Configuration();
                SaveConfig();
            }
        }

        private void SaveConfig()
        {
            Configuration.Serialize(ConfigPath, _config);
        }

        private bool _resizing = false;
        private Vector2 _resizeHandle = Vector2.zero;
        private bool _moving = false;
        private Vector2 _moveHandle = Vector2.zero;

        private UIPanel[] _panels;

        private static readonly string[] PanelNames = new string[]
		{
			"RoadsSmallPanel",
			"RoadsMediumPanel",
			"RoadsLargePanel",
			"RoadsHighwayPanel",
			"RoadsIntersectionPanel",
            //"Some RoadsPanel",
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

        void UpdatePanel(UIPanel panel)
        {
            var tabContainer = panel.gameObject.transform.parent.GetComponent<UITabContainer>();
            if (!_config.PanelPositionSet)
            {
                _config.PanelPosition = tabContainer.relativePosition;
                _config.PanelSize = tabContainer.size;
                _config.PanelPositionSet = true;
            }

            var scrollablePanel = panel.Find<UIScrollablePanel>("ScrollablePanel");
            var itemCount = scrollablePanel.transform.childCount;

            tabContainer.relativePosition = _config.PanelPosition;
            tabContainer.size = _config.PanelSize;

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
            scrollBar.autoHide = false;
            scrollBar.size = new Vector2(20.0f, tabContainer.size.y - 26.0f);
            scrollBar.orientation = UIOrientation.Vertical;
            scrollBar.isInteractive = true;
            scrollBar.isVisible = true;
            scrollBar.enabled = true;
            scrollBar.relativePosition = new Vector3(tabContainer.size.x - 20.0f - 2.0f, 0.0f, 0);

            // ******* This is what determines the scrolling speed. We will set this in the XML file, but in case it's not //
            // ******* there, we will set it to a default.
            if (_config.ScrollSpeed <= 10)
            {
                _config.ScrollSpeed = 160;
             }
            scrollBar.incrementAmount = _config.ScrollSpeed;

            try
            {
                scrollBar.Find<UIButton>("ArrowLeft").isVisible = false;
                scrollBar.Find<UIButton>("ArrowRight").isVisible = false;
            }
            catch (Exception ex)
            {
                Debug.Log(String.Format("Exception: {0}", ex));

            }

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
                    _resizing = true;
                    _resizeHandle = Input.mousePosition;
                };

                resizeButton.eventMouseUp += (component, param) =>
                {
                    resizeButton.color = Color.white;
                    _resizing = false;
                    _resizeHandle = Vector2.zero;
                    SaveConfig();
                };
            }

            resizeButton.relativePosition = new Vector3(0.0f, scrollBar.size.y, 0.0f);

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
                        _moving = true;
                        _moveHandle = Input.mousePosition;
                    }
                };

                scrollablePanel.eventMouseUp += (component, param) =>
                {
                    _moving = false;
                    _moveHandle = Vector2.zero;
                    SaveConfig();
                };
            }

            scrollablePanel.size = new Vector2(tabContainer.size.x - 32.0f, _config.PanelSize.y - 2.0f);

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

        private UIPanel _openPanel = null;

        void Start()
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
                //  panels[i] = GameObject.Find(panelNames[i]).GetComponent<UIPanel>();

                catch (Exception)
                {
                    Debug.Log(String.Format("Couldn't find panel with name {0}", PanelNames[i]));
                }
            }
        }
        


        void Update()
        {
            try
            {
                if (_openPanel != null)
                {
                    if (!_openPanel.isVisible)
                    {
                        _openPanel = null;
                    }
                }

                if (_resizing)
                {
                    Vector2 pos = Input.mousePosition;
                    var delta = pos - _resizeHandle;
                    _resizeHandle = pos;
                    _config.PanelSize += new Vector2(delta.x, -delta.y);

                    if (_config.PanelSize.x <= 64.0f)
                    {
                        _config.PanelSize = new Vector2(64.0f, _config.PanelSize.y);
                    }

                    if (_config.PanelSize.y <= 64.0f)
                    {
                        _config.PanelSize = new Vector2(_config.PanelSize.x, 64.0f);
                    }

                    _openPanel = null;
                }
                else if (_moving)
                {
                    Vector2 pos = Input.mousePosition;
                    var delta = pos - _moveHandle;
                    _moveHandle = pos;
                    _config.PanelPosition += new Vector2(delta.x, -delta.y);

                    _openPanel = null;
                }

                if (_openPanel == null)
                {
                    foreach (var panel in _panels)
                    {
                        if (panel.isVisible)
                        {
                            _openPanel = panel;
                            break;
                        }
                    }

                    if (_openPanel != null)
                    {
                        UpdatePanel(_openPanel);
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
