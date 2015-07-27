using System;
using System.Collections.Generic;
using System.Linq;
using EnhancedBuildPanel;
using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace EnhancedBuildPanel.GUI_2
{
    public class UIMainPanel : UIPanel
    {


        private bool _showDefault;
        private UITitleBar _title;
        //private event MouseEventHandler eventClick;
        private RoadsOptionPanel roadsOptionPanel = new RoadsOptionPanel();
        ////private readonly UIGetButtons _uiGetButtons;

        private UIScrollablePanel _scrollablePanel;
        private UIScrollbar scrollBar;
        private UISlicedSprite trackSprite;
        private UISlicedSprite thumbSprite;
        public override void Start()
        {

            name = EnhancedBuildPanel.Acronym;
            //name = "myMod";
            atlas = UIUtils.GetAtlas("Ingame");
            backgroundSprite = "UnlockingPanel2";
            size = new Vector2(850, 850);
            pivot = UIPivotPoint.TopLeft;
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            width = 1900f;
            height = 950f;
            m_MaxSize = new Vector2(1200, 850);
            m_MinSize = new Vector2(200, 100);
            autoFitChildrenHorizontally = true;
            relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2),
                Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            Debug.Log("start setup controls");


            //Setup scrollable Panel ... this should be fun ..
            Debug.Log("Creating Scrollable panel2");
            _scrollablePanel = AddUIComponent<UIScrollablePanel>();
            _scrollablePanel.transform.parent = transform;
            Debug.Log(string.Format("parent of scrollable panel is {0}", _scrollablePanel.parent.name));
            Debug.Log("Naming scrollable panel");
            _scrollablePanel.name = EnhancedBuildPanel.Acronym + "_ScrollablePanel";
            _scrollablePanel.scrollWheelDirection = UIOrientation.Vertical;
            _scrollablePanel.horizontalScrollbar = null;
            Debug.Log("Setting scrollbar ");
            _scrollablePanel.verticalScrollbar = scrollBar;
            
            _scrollablePanel.scrollWheelAmount = 16;
            _scrollablePanel.autoLayout = false;
            _scrollablePanel.autoSize = false;
            Debug.Log("setting relative position of scrollable panel");
            _scrollablePanel.relativePosition = new Vector3(2.0f, 2.0f, 0.0f);
            Debug.Log(string.Format("setting width of scrollable panel currently at {0}", _scrollablePanel.width));

            //_scrollablePanel.width = 750f;
            Debug.Log(string.Format("set based on parent width which is currently set to {0}", _scrollablePanel.parent.width));
            _scrollablePanel.width = (_scrollablePanel.parent.width - 10.0f);
            Debug.Log("setting height of scrollable panel");
            //_scrollablePanel.height = 
            _scrollablePanel.height = (_scrollablePanel.parent.height - 10.0f);
            
            
            _scrollablePanel.autoLayout = true;
            
            //_scrollablePanel.FitChildrenHorizontally(5.0f);
            _scrollablePanel.BringToFront();
            
/*            
            _scrollablePanel.scrollPosition = new Vector2(0.0f, 16.0f);

            Debug.Log(string.Format("current scroll position is {0}", _scrollablePanel.scrollPosition));
            
            _scrollablePanel.eventMouseWheel += (component, param) =>
            {
                _scrollablePanel.scrollPosition = new Vector2(0.0f, _scrollablePanel.scrollPosition.y + -param.wheelDelta * 16.0f);
            };

            scrollBar.eventValueChanged += delegate(UIComponent component, float value)
            {
                _scrollablePanel.scrollPosition = new Vector2(0.0f, value);
            };
  */         
           Debug.Log("shit");

            scrollBar = AddUIComponent<UIScrollbar>();
            Debug.Log("setting parent of scrollbar");
            scrollBar.transform.parent = _scrollablePanel.transform;
            Debug.Log("setting properties of scrollbar");
            scrollBar.autoHide = false;
            Debug.Log("set size of scrollbar");
            scrollBar.size = new Vector2(20.0f, 50.0f);
            Debug.Log("Set orientation of scrollbar");
            scrollBar.orientation = UIOrientation.Vertical;
            Debug.Log("is test interactive scrollbar");
            scrollBar.isInteractive = true;
            Debug.Log("set is visible scrollbar");
            scrollBar.isVisible = true;
            scrollBar.enabled = true;

            scrollBar.relativePosition = new Vector3(_scrollablePanel.size.x - 20.0f - 2.0f, 0.0f, 0);
            Debug.Log("set increment amount scrollbar");
            scrollBar.incrementAmount = 10;

            Debug.Log("setting up track sprite");
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

            trackSprite.size = scrollBar.size;
            thumbSprite.width = trackSprite.width;

            _scrollablePanel.scrollPosition = new Vector2(0.0f, 16.0f);

            Debug.Log(string.Format("current scroll position is {0}", _scrollablePanel.scrollPosition));

            _scrollablePanel.eventMouseWheel += (component, param) =>
            {
                _scrollablePanel.scrollPosition = new Vector2(0.0f, _scrollablePanel.scrollPosition.y + -param.wheelDelta * 16.0f);
            };

            scrollBar.eventValueChanged += delegate(UIComponent component, float value)
            {
                _scrollablePanel.scrollPosition = new Vector2(0.0f, value);
            };

  
            
            base.Start(); 

            SetupControls();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }


        public override void Update()
        {
            base.Update();
            if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.P))
            {
                
                Debug.Log("setup controls with hotkey");
                isVisible = !isVisible;

                if (isVisible)
                {
                    BringToFront();
                }
                else
                {
                    _showDefault = false;
                }
            }
            



        }




        UIComponent _uiComponent;
      //  UIButton cloneButton;
        
        
        private string prefix = EnhancedBuildPanel.Acronym + "_";

        public delegate void ButtonClicked();

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
                    Debug.Log(string.Format("creating clone of {0}", button));
                    UIButton cloneButton = Instantiate(button);
                    Debug.Log(string.Format("Setting fields for cloned object"));
                    //cloneButton = button.GetComponent<UIComponent>();
                    var buttonName = "EBP_" + button.name;
                    
                    cloneButton.name = buttonName;
                    Debug.Log(string.Format("attaching event onButtonclicked to button {0}", cloneButton));
                    cloneButton.eventClick += setTool;
                    //Debug.Log("Attached Event");
                    


                    /*
                    Debug.Log(string.Format("Getting eventinfo for {0}", button));
                    var eventInfo = button.GetType().GetEvent("eventClick");
                    Debug.Log(string.Format("getting method info for {0}", button));

                    var methodInfo = button.GetType().GetMethod("OnClick");
                    
                    Debug.Log(string.Format("Creating delegate"));
                    Delegate handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, button, methodInfo);
                    eventInfo.AddEventHandler(cloneButton, handler);
                    */
               //   cloneButton = _scrollablePanel.AddUIComponent<UIButton>();
                    Debug.Log("Adding to Panel");
                    //cloneButton = AddUIComponent<UIButton>();
                    Debug.Log(string.Format("scrollable panel name is {0}", _scrollablePanel));
                    cloneButton.transform.parent = _scrollablePanel.transform;
                    //cloneButton = AddUIComponent<UIButton>();
                    //cloneButton.transform.parent = transform;

                    Debug.Log(string.Format("Cloned button {0} to button {1}  and attached it to {3}!", button,cloneButton,cloneButton.parent));



                }
                catch (Exception e)
                {
                    Debug.Log(string.Format("Exception cloning buttons : {0}", e));
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

        private void setTool(UIComponent component, UIMouseEventParameter eventParam)
        {


            string oldName = component.name.Remove(0, (EnhancedBuildPanel.Acronym.Length +1 ));
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

        private void SetupControls()
        {
          //_scrollablePanel = AddUIComponent<UIScrollablePanel>();
            
            _title = AddUIComponent<UITitleBar>();
            _title.iconSprite = "IconAssetBuilding";
            _title.title = "Enhanced Build Panel" + EnhancedBuildPanel.Version;
            var offset = 40f;
            UIButtons("RoadsSmallPanel");



             UIButtons("RoadsMediumPanel");
             UIButtons("RoadsLargePanel");
            // UIButtons("RoadsHighwayPanel");
            // UIButtons("RoadsIntersectionPanel");
            //UIButtons("ElectricityDefaultPanel");
            //UIGetButtons.UIButtons("WaterAndSewagePanel");
        }

        protected void onButtonClicked(UIComponent component, UIMouseEventParameter p)
        {

            object objectUserData = component.objectUserData;
            NetInfo netInfo = objectUserData as NetInfo;
            BuildingInfo buildingInfo = objectUserData as BuildingInfo;
            if ((UnityEngine.Object) this.roadsOptionPanel != (UnityEngine.Object) null)
                this.roadsOptionPanel.Show();
            NetTool netTool = ToolsModifierControl.SetTool<NetTool>();
            if ((UnityEngine.Object) netTool != (UnityEngine.Object) null)
            {
                netTool.m_prefab = netInfo;
            }
            if (!((UnityEngine.Object) buildingInfo != (UnityEngine.Object) null))
                return;
            if ((UnityEngine.Object) this.roadsOptionPanel != (UnityEngine.Object) null)
                this.roadsOptionPanel.Hide();
            BuildingTool buildingTool = ToolsModifierControl.SetTool<BuildingTool>();
            if (!((UnityEngine.Object) buildingTool != (UnityEngine.Object) null))
                return;
            buildingTool.m_prefab = buildingInfo;
            buildingTool.m_relocate = 0;
        }
    }
}





#region blah

        // Title Bar

/*
            // Type DropDown
            UILabel label = AddUIComponent<UILabel>();
            label.textScale = 0.8f;
            label.padding = new RectOffset(0, 0, 8, 0);
            label.relativePosition = new Vector3(15f, offset + 5f);
            label.text = "Type :";

            _prefabType = UIUtils.CreateDropDown(this);
            _prefabType.width = 110;
            _prefabType.AddItem("Roads");
            _prefabType.AddItem("Electrical");
            _prefabType.AddItem("Water");
            _prefabType.selectedIndex = 0;
            _prefabType.relativePosition = label.relativePosition + new Vector3(60f, 0f);
/*
            _prefabType.eventSelectedIndexChanged += (c, t) =>
            {
                _prefabType.enabled = false;
                _isSorted = false;
                PopulateList();
                _prefabType.enabled = true;
            };

            // Sorting DropDown
            label = AddUIComponent<UILabel>();
            label.textScale = 0.8f;
            label.padding = new RectOffset(0, 0, 8, 0);
            label.relativePosition = _prefabType.relativePosition + new Vector3(130f, 0f);
            label.text = "Sort by :";

            _sorting = UIUtils.CreateDropDown(this);
            _sorting.width = 125;
            _sorting.AddItem("Name");
            _sorting.selectedIndex = 0;
            _sorting.relativePosition = label.relativePosition + new Vector3(60f, 0f);

  /*          _sorting.eventSelectedIndexChanged += (c, t) =>
            {
                _sorting.enabled = false;
                _isSorted = false;
                PopulateList();
                _sorting.enabled = true;
            };

            // Sorting direction
            _sortDirection = AddUIComponent<UISprite>();
            _sortDirection.atlas = UIUtils.defaultAtlas;
            _sortDirection.spriteName = "IconUpArrow";
            _sortDirection.relativePosition = _sorting.relativePosition + new Vector3(130f, 0f);

/*            _sortDirection.eventClick += (c, t) =>
            {
                _sortDirection.flip = (_sortDirection.flip == UISpriteFlip.None) ? UISpriteFlip.FlipVertical : UISpriteFlip.None;
                _isSorted = false;
                PopulateList();
            };

            // Search
            _search = UIUtils.CreateTextField(this);
            _search.width = 150f;
            _search.height = 30f;
            _search.padding = new RectOffset(6, 6, 6, 6);
            _search.relativePosition = new Vector3(width - _search.width - 15f, offset + 5f);

            label = AddUIComponent<UILabel>();
            label.textScale = 0.8f;
            label.padding = new RectOffset(0, 0, 8, 0);
            label.relativePosition = _search.relativePosition - new Vector3(60f, 0f);
            label.text = "Search :";


 //           _search.eventTextChanged += (c, t) => PopulateList();

            // Labels
            label = AddUIComponent<UILabel>();
            label.textScale = 0.9f;
            label.text = "Name";
            label.relativePosition = new Vector3(15f, offset + 50f);

            label = AddUIComponent<UILabel>();
            label.textScale = 0.9f;
            label.text = "Texture";
            label.relativePosition = new Vector3(width - 135f, offset + 50f);

            UILabel label2 = AddUIComponent<UILabel>();
            label2.textScale = 0.9f;
            label2.text = "Weight";
            label2.relativePosition = label.relativePosition - new Vector3(125f, 0f);

            label = AddUIComponent<UILabel>();
            label.textScale = 0.9f;
            label.text = "Triangles";
            label.relativePosition = label2.relativePosition - new Vector3(115f, 0f);

            // Item List
            //_itemList = UIFastList.Create<UIPrefabItem>(this);
            
            _itemList.rowHeight = 40f;
            _itemList.backgroundSprite = "UnlockingPanel";
            _itemList.width = width - 10;
            _itemList.height = height - offset - 75;
            _itemList.relativePosition = new Vector3(5f, offset + 70f);
        }
        */
        /*
        private GetPanelButtons GetPanelButtons()
        {
            throw new NotImplementedException();
        }
        
        private void InitializePreafabLists()
        {
            _isSorted = false;

            int prefabCount = PrefabCollection<BuildingInfo>.PrefabCount();
            int count = 0;
            int maxCount = prefabCount;

            // Buildings
            m_buildingPrefabs = new RenderGroup.MeshData[prefabCount];
            for (uint i = 0; i < prefabCount; i++)
            {
                PrefabInfo prefab = PrefabCollection<BuildingInfo>.GetPrefab(i);
                if (prefab != null && (_showDefault || prefab.name.Contains(".")))
                {
                    if ((prefab as BuildingInfo).m_mesh == null || !(prefab as BuildingInfo).m_mesh.isReadable) continue;
                    m_buildingPrefabs[count++] = new RenderGroup.MeshData(prefab);
                }
            }
            Array.Resize<RenderGroup.MeshData>(ref m_buildingPrefabs, count);

            // Props
            prefabCount = PrefabCollection<PropInfo>.PrefabCount();
            count = 0;
            maxCount = Math.Max(maxCount, prefabCount);
            m_propPrefabs = new RenderGroup.MeshData[prefabCount];
            for (uint i = 0; i < prefabCount; i++)
            {
                PrefabInfo prefab = PrefabCollection<PropInfo>.GetPrefab(i);
                if (prefab != null && (_showDefault || prefab.name.Contains(".")))
                {
                    if ((prefab as PropInfo).m_mesh == null || !(prefab as PropInfo).m_mesh.isReadable) continue;
                    m_propPrefabs[count++] = new RenderGroup.MeshData(prefab);
                }
            }
            Array.Resize<RenderGroup.MeshData>(ref m_propPrefabs, count);

            // Trees
            prefabCount = PrefabCollection<TreeInfo>.PrefabCount();
            count = 0;
            maxCount = Math.Max(maxCount, prefabCount);
            m_treePrefabs = new RenderGroup.MeshData[prefabCount];
            for (uint i = 0; i < prefabCount; i++)
            {
                PrefabInfo prefab = PrefabCollection<TreeInfo>.GetPrefab(i);
                if (prefab != null && (_showDefault || prefab.name.Contains(".")))
                {
                    if ((prefab as TreeInfo).m_mesh == null || !(prefab as TreeInfo).m_mesh.isReadable) continue;
                    m_treePrefabs[count++] = new RenderGroup.MeshData(prefab);
                }
            }
            Array.Resize<RenderGroup.MeshData>(ref m_treePrefabs, count);

            // Vehicles
            prefabCount = PrefabCollection<VehicleInfo>.PrefabCount();
            count = 0;
            maxCount = Math.Max(maxCount, prefabCount);
            m_vehiclePrefabs = new RenderGroup.MeshData[prefabCount];
            for (uint i = 0; i < prefabCount; i++)
            {
                PrefabInfo prefab = PrefabCollection<VehicleInfo>.GetPrefab(i);
                if (prefab != null && (_showDefault || prefab.name.Contains(".")))
                {
                    if ((prefab as VehicleInfo).m_mesh == null || !(prefab as VehicleInfo).m_mesh.isReadable) continue;
                    m_vehiclePrefabs[count++] = new RenderGroup.MeshData(prefab);
                }
            }
            Array.Resize<RenderGroup.MeshData>(ref m_vehiclePrefabs, count);

            PopulateList();
        }

        private void PopulateList()
        {
            RenderGroup.MeshData[] prefabList = null;

            int index = _prefabType.selectedIndex;
            switch (index)
            {
                case 0:
                    prefabList = m_buildingPrefabs;
                    break;
                case 1:
                    prefabList = m_propPrefabs;
                    break;
                case 2:
                    prefabList = m_treePrefabs;
                    break;
                case 3:
                    prefabList = m_vehiclePrefabs;
                    break;
            }

            if (prefabList == null) return;

            // Filtering
            string filter = _search.text.Trim().ToLower();
            if (!String.IsNullOrEmpty(filter))
            {
                RenderGroup.MeshData[] filterList = new RenderGroup.MeshData[prefabList.Length];
                int count = 0;

                for (int i = 0; i < prefabList.Length; i++)
                {
                    if (prefabList[i].name.ToLower().Contains(filter) || (prefabList[i].steamID != null && prefabList[i].steamID.Contains(filter)))
                    {
                        filterList[count++] = prefabList[i];
                    }
                }

                Array.Resize<RenderGroup.MeshData>(ref filterList, count);
                prefabList = filterList;
            }

            // Sorting
            if (!_isSorted)
            {
                RenderGroup.MeshData.sorting = (RenderGroup.MeshData.Sorting)_sorting.selectedIndex;
                RenderGroup.MeshData.ascendingSort = (_sortDirection.flip == UISpriteFlip.None);
                Array.Sort(prefabList);

                _isSorted = true;
            }

            // Display
            _itemList.rowsData.m_buffer = prefabList;
            _itemList.rowsData.m_size = prefabList.Length;

            _itemList.DisplayAt(0);
        }
         */

        /* This section will go through each panel passed to it and clone all the buttons
         */

        #endregion blah
