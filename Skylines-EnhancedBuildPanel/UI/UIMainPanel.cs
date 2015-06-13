using System;
using ColossalFramework.UI;
using UnityEngine;

namespace EnhancedBuildPanel.UI
{
    public class UIMainPanel : UIPanel
    {
        private UITitleBar _title;
        private UIDropDown _assetType;
        private UIDropDown _sort;
        private UISprite _sortOrder;

        private UIFastList _itemList;
        private AssetData _assetData;

        private AssetData[] _roads;
        private AssetData[] _fire;
        private AssetData[] _garbage;
        private AssetData[] _electrical;

        private bool _showDefault;

        private bool _isSorted;
//        private const int MaxIterations = 10;

        public override void Start()
        {
            base.Start();

            backgroundSprite = "UnlockingPanel2";
            isVisible = false;
            canFocus = true;
            isInteractive = true;
            width = 770;
            height = 475;
            relativePosition = new Vector3(Mathf.Floor((GetUIView().fixedWidth - width) / 2), Mathf.Floor((GetUIView().fixedHeight - height) / 2));
            
            SetupControls();

        }

        public override void Update()
        {
            base.Update();

            // Super secret key combination
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.M))
            {
                isVisible = true;
                BringToFront();

                _showDefault = !_showDefault;
                InitializePreafabLists();
            }
            else if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) && Input.GetKeyDown(KeyCode.M))
            {
                isVisible = !isVisible;

                if (isVisible)
                {
                    InitializePreafabLists();
                    BringToFront();
                }
                else
                {
                    _showDefault = false;
                }
            }
        }

        private void SetupControls()
        {
            float offset = 40f;

            // Title Bar
            _title = AddUIComponent<UITitleBar>();
            _title.IconSprite = "IconAssetBuilding";
            _title.Title = "Mesh Info " + EnhancedBuildPanel.Version;

            // Type DropDown
            UILabel label = AddUIComponent<UILabel>();
            label.textScale = 0.8f;
            label.padding = new RectOffset(0, 0, 8, 0);
            label.relativePosition = new Vector3(15f, offset + 5f);
            label.text = "Type :";

            _assetType = UIUtils.CreateDropDown(this);
            _assetType.width = 110;
            _assetType.AddItem("Building");
            _assetType.AddItem("Prop");
            _assetType.AddItem("Tree");
            _assetType.AddItem("Vehicle");
            _assetType.selectedIndex = 0;
            _assetType.relativePosition = label.relativePosition + new Vector3(60f, 0f);

            _assetType.eventSelectedIndexChanged += (c, t) =>
            {
                _assetType.enabled = false;
                _isSorted = false;
                PopulateList();
                _assetType.enabled = true;
            };

            // Sorting DropDown
            label = AddUIComponent<UILabel>();
            label.textScale = 0.8f;
            label.padding = new RectOffset(0, 0, 8, 0);
            label.relativePosition = _assetType.relativePosition + new Vector3(130f, 0f);
            label.text = "Sort by :";

            _sort = UIUtils.CreateDropDown(this);
            _sort.width = 125;
            _sort.AddItem("Name");
            _sort.AddItem("Triangles");
            _sort.AddItem("LOD Triangles");
            _sort.AddItem("Weight");
            _sort.AddItem("LOD Weight");
            _sort.AddItem("Texture");
            _sort.AddItem("LOD Texture");
            _sort.selectedIndex = 0;
            _sort.relativePosition = label.relativePosition + new Vector3(60f, 0f);

            _sort.eventSelectedIndexChanged += (c, t) =>
            {
                _sort.enabled = false;
                _isSorted = false;
                PopulateList();
                _sort.enabled = true;
            };

            // Sorting direction
            _sortOrder = AddUIComponent<UISprite>();
            _sortOrder.spriteName = "IconUpArrow";
            _sortOrder.relativePosition = _sort.relativePosition + new Vector3(130f, 0f);

            _sortOrder.eventClick += (c, t) =>
            {
                _sortOrder.flip = (_sortOrder.flip == UISpriteFlip.None) ? UISpriteFlip.FlipVertical : UISpriteFlip.None;
                _isSorted = false;
                PopulateList();
            };
        }


        private void InitializePreafabLists()
        {
            _isSorted = false;

            int prefabCount = PrefabCollection<NetInfo>.PrefabCount();
            int count = 0;
            int maxCount = prefabCount;

            _roads = new AssetData[prefabCount];

            for (uint i = 0; i < prefabCount; i++)
            {
                PrefabInfo prefab = PrefabCollection<NetInfo>.GetPrefab(i);
                if (prefab != null && (_showDefault || prefab.name.Contains(".")))
                {
                    _roads[count++] = new AssetData(prefab);
                }
            }
            PopulateList();
        }

        private void PopulateList()
        {

            AssetData[] prefabList = null;
            int index = _assetType.selectedIndex;
            switch (index)
            {
                case 0:
                    prefabList = _roads;
                    break;
                case 1:
                    prefabList = _fire;
                    break;
                case 2:
                    prefabList = _garbage;
                    break;
                case 3:
                    prefabList = _electrical;
                    break;
            }

            // Display
       _itemList.RowsData.m_buffer = prefabList;
        _itemList.RowsData.m_size = prefabList.Length;

            _itemList.DisplayAt(0);
        }
    }
}
