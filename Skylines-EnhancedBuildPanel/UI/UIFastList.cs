using System;
using ColossalFramework.UI;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections.Generic;

namespace EnhancedBuildPanel.UI
{
    public interface IUIFastListRow
    {
        #region Methods to implement
        /// <summary>
        /// Method invoked very often, make sure it is fast
        /// Avoid doing any calculations, the data should be already processed any ready to display.
        /// </summary>
        /// <param name="data">What needs to be displayed</param>
        /// <param name="isRowOdd">Use this to display a different look for your odd rows</param>
        void Display(object data, bool isRowOdd);

        /// <summary>
        /// Change the style of the selected row here
        /// </summary>
        /// <param name="isRowOdd">Use this to display a different look for your odd rows</param>
        void Select(bool isRowOdd);
        /// <summary>
        /// Change the style of the row back from selected here
        /// </summary>
        /// <param name="isRowOdd">Use this to display a different look for your odd rows</param>
        void Deselect(bool isRowOdd);
        #endregion

        #region From UIPanel
        // No need to implement those, they are in UIPanel
        // Those are declared here so they can be used inside UIFastList
        bool Enabled { get; set; }
        Vector3 RelativePosition { get; set; }
        event MouseEventHandler EventClick;
        #endregion
    }

    /// <summary>
    /// This component is specifically designed the handle the display of
    /// very large amount of rows in a scrollable panel while minimizing
    /// the impact on the performances.
    /// 
    /// This class will instantiate the rows for you based on the actual
    /// height of the UIFastList and the rowHeight value provided.
    /// 
    /// The row class must inherit UIPanel and implement IUIFastListRow :
    /// public class MyCustomRow : UIPanel, IUIFastListRow
    /// 
    /// How it works :
    /// This class only instantiate as many rows as visible on screen (+1
    /// extra to simulate in-between steps). Then the content of those is
    /// updated according to what needs to be displayed by calling the
    /// Display method declared in IUIFastListRow.
    /// 
    /// Provide the list of data with rowData. This data is send back to
    /// your custom row when it needs to be displayed. For optimal
    /// performances, make sure this data is already processed and ready
    /// to display.
    /// 
    /// Creation example :
    /// UIFastList myFastList = UIFastList.Create<MyCustomRow>(this);
    /// myFastList.size = new Vector2(200f, 300f);
    /// myFastList.rowHeight = 40f;
    /// myFastList.rowData = myDataList;
    /// 
    /// </summary>
    public class UIFastList : UIComponent
    {
        #region Private members
        private UIPanel _mPanel;
        private UIScrollbar _mScrollbar;
        private FastList<IUIFastListRow> _mRows;
        private FastList<object> _mRowsData;

        private Type _mRowType;
        private string _mBackgroundSprite;
        private Color32 _mColor = new Color32(255, 255, 255, 255);
        private float _mRowHeight = -1;
        private float _mPos = -1;
        private float _mStepSize = 0;
        private bool _mCanSelect = false;
        private int _mSelectedDataId = -1;
        private int _mSelectedRowId = -1;
        private bool _mLock = false;
        private bool _mUpdateContent = true;
        #endregion

        /// <summary>
        /// Use this to create the UIFastList.
        /// Do NOT use AddUIComponent.
        /// I had to do that way because MonoBehaviors classes cannot be generic
        /// </summary>
        /// <typeparam name="T">The type of the row UI component</typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static UIFastList Create<T>(UIComponent parent)
            where T : UIPanel, IUIFastListRow
        {
            UIFastList list = new UIFastList {_mRowType = typeof (T)};
            return list;
        }

        #region Public accessors
        /// <summary>
        /// Change the color of the background
        /// </summary>
        public Color32 BackgroundColor
        {
            get { return _mColor; }
            set
            {
                _mColor = value;
                if (_mPanel != null)
                    _mPanel.color = value;
            }
        }

        /// <summary>
        /// Change the sprite of the background
        /// </summary>
        public string BackgroundSprite
        {
            get { return _mBackgroundSprite; }
            set
            {
                if (_mBackgroundSprite != value)
                {
                    _mBackgroundSprite = value;
                    if (_mPanel != null)
                        _mPanel.backgroundSprite = value;
                }
            }
        }

        /// <summary>
        /// Can rows be selected by clicking on them
        /// Default value is false
        /// Rows can still be selected via selectedIndex
        /// </summary>
        public bool CanSelect
        {
            get { return _mCanSelect; }
            set
            {
                if (_mCanSelect != value)
                {
                    _mCanSelect = value;

                    if (_mRows == null) return;
                    for (int i = 0; i < _mRows.m_size; i++)
                    {
                        if (_mCanSelect)
                            _mRows[i].EventClick += OnRowClicked;
                        else
                            _mRows[i].EventClick -= OnRowClicked;
                    }
                }
            }
        }

        /// <summary>
        /// Change the position in the list
        /// Display the data at the position in the top row.
        /// This doesn't update the list if the position stay the same
        /// Use DisplayAt for that
        /// </summary>
        public float ListPosition
        {
            get { return _mPos; }
            set
            {
                if (_mRowHeight <= 0) return;
                if (_mPos != value)
                {
                    float pos = Mathf.Max(Mathf.Min(value, _mRowsData.m_size - height / _mRowHeight), 0);
                    _mUpdateContent = Mathf.FloorToInt(_mPos) != Mathf.FloorToInt(pos);
                    DisplayAt(pos);
                }
            }
        }

        /// <summary>
        /// This is the list of data that will be send to the IUIFastListRow.Display method
        /// Changing this list will reset the display position to 0
        /// You can also change rowsData.m_buffer and rowsData.m_size
        /// and refresh the display with DisplayAt method
        /// </summary>
        public FastList<object> RowsData
        {
            get
            {
                if (_mRowsData == null) _mRowsData = new FastList<object>();
                return _mRowsData;
            }
            set
            {
                if (_mRowsData != value)
                {
                    _mRowsData = value;
                    DisplayAt(0);
                }
            }
        }

        /// <summary>
        /// This MUST be set, it is the height in pixels of each row
        /// </summary>
        public float RowHeight
        {
            get { return _mRowHeight; }
            set
            {
                if (_mRowHeight != value)
                {
                    _mRowHeight = value;
                    CheckRows();
                }
            }
        }

        /// <summary>
        /// Currently selected row
        /// -1 if none selected
        /// </summary>
        public int SelectedIndex
        {
            get { return _mSelectedDataId; }
            set
            {
                if (_mRowsData == null) return;

                int oldId = _mSelectedDataId;
                if (oldId >= _mRowsData.m_size) oldId = -1;
                _mSelectedDataId = Mathf.Min(Mathf.Max(-1, value), _mRowsData.m_size - 1);

                int pos = Mathf.FloorToInt(_mPos);
                int newRowId = Mathf.Max(-1, _mSelectedDataId - pos);
                if (newRowId >= _mRows.m_size) newRowId = -1;

                if (newRowId >= 0 && newRowId == _mSelectedRowId && !_mUpdateContent) return;

                if (_mSelectedRowId >= 0)
                {
                    _mRows[_mSelectedRowId].Deselect((oldId % 2) == 1);
                    _mSelectedRowId = -1;
                }

                if (newRowId >= 0)
                {
                    _mSelectedRowId = newRowId;
                    _mRows[_mSelectedRowId].Select((_mSelectedDataId % 2) == 1);
                }

                if (EventSelectedIndexChanged != null && _mSelectedDataId != oldId)
                    EventSelectedIndexChanged(this, _mSelectedDataId);
            }
        }

        /// <summary>
        /// The number of pixels moved at each scroll step
        /// When set to 0 or less, rowHeight is used instead.
        /// </summary>
        public float StepSize
        {
            get { return (_mStepSize > 0) ? _mStepSize : _mRowHeight; }
            set { _mStepSize = value; }
        }
        #endregion

        #region Events
        /// <summary>
        /// Called when the currently selected row changed
        /// </summary>
        public event PropertyChangedEventHandler<int> EventSelectedIndexChanged;
        #endregion

        #region Public methods
        /// <summary>
        /// Clear the list
        /// </summary>
        public void Clear()
        {
            _mRowsData.Clear();

            for (int i = 0; i < _mRows.m_size; i++)
            {
                _mRows[i].Enabled = false;
            }

            UpdateScrollbar();
        }

        /// <summary>
        /// Display the data at the position in the top row.
        /// This update the list even if the position remind the same
        /// </summary>
        /// <param name="pos">Index position in the list</param>
        public void DisplayAt(float pos)
        {
            if (_mRowsData == null || _mRowHeight <= 0) return;

            _mPos = Mathf.Max(Mathf.Min(pos, _mRowsData.m_size - height / _mRowHeight), 0f);

            for (int i = 0; i < _mRows.m_size; i++)
            {
                int dataPos = Mathf.FloorToInt(_mPos + i);
                float offset = RowHeight * (_mPos + i - dataPos);
                if (dataPos < _mRowsData.m_size)
                {
                    if (_mUpdateContent)
                        _mRows[i].Display(_mRowsData[dataPos], (dataPos % 2) == 1);

                    if (dataPos == _mSelectedDataId && _mUpdateContent)
                    {
                        _mSelectedRowId = i;
                        _mRows[_mSelectedRowId].Select((dataPos % 2) == 1);
                    }

                    _mRows[i].Enabled = true;
                    _mRows[i].RelativePosition = new Vector3(0, i * RowHeight - offset);
                }
                else
                    _mRows[i].Enabled = false;
            }
            UpdateScrollbar();
            _mUpdateContent = true;
        }
        #endregion

        #region Overrides
        public override void Start()
        {
            base.Start();

            SetupControls();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            if (_mPanel == null) return;

            Destroy(_mPanel);
            Destroy(_mScrollbar);

            if (_mRows == null) return;

            for (int i = 0; i < _mRows.m_size; i++)
            {
                Destroy(_mRows[i] as Object);
            }
        }

        protected override void OnSizeChanged()
        {
            base.OnSizeChanged();

            if (_mPanel == null) return;

            _mPanel.size = size;

            _mScrollbar.height = height;
            _mScrollbar.trackObject.height = height;
            _mScrollbar.AlignTo(this, UIAlignAnchor.TopRight);

            CheckRows();
        }

        protected override void OnMouseWheel(UIMouseEventParameter p)
        {
            base.OnMouseWheel(p);

            if (_mStepSize > 0 && _mRowHeight > 0)
                ListPosition = _mPos - p.wheelDelta * _mStepSize / _mRowHeight;
            else
                ListPosition = _mPos - p.wheelDelta;
        }
        #endregion

        #region Private methods

        protected void OnRowClicked(UIComponent component, UIMouseEventParameter p)
        {
            for (int i = 0; i < RowsData.m_size; i++)
            {
                if (component == (UIComponent)_mRows[i])
                {
                    SelectedIndex = i + Mathf.FloorToInt(_mPos);
                    return;
                }
            }
        }

        private void CheckRows()
        {
            if (_mPanel == null || _mRowHeight <= 0) return;

            int nbRows = Mathf.CeilToInt(height / _mRowHeight) + 1;

            if (_mRows == null)
            {
                _mRows = new FastList<IUIFastListRow>();
                _mRows.SetCapacity(nbRows);
            }

            if (_mRows.m_size < nbRows)
            {
                // Adding missing rows
                for (int i = _mRows.m_size; i < nbRows; i++)
                {
                    _mRows.Add(_mPanel.AddUIComponent(_mRowType) as IUIFastListRow);
                    if (_mCanSelect) _mRows[i].EventClick += OnRowClicked;
                }
            }
            else if (_mRows.m_size > nbRows)
            {
                // Remove excess rows
                for (int i = nbRows; i < _mRows.m_size; i++)
                    Destroy(_mRows[i] as Object);

                _mRows.SetCapacity(nbRows);
            }

            UpdateScrollbar();
        }

        private void UpdateScrollbar()
        {
            if (_mRowsData == null || _mRowHeight <= 0) return;

            float h = _mRowHeight * _mRowsData.m_size;
            float scrollSize = height * height / (_mRowHeight * _mRowsData.m_size);
            float amount = StepSize * height / (_mRowHeight * _mRowsData.m_size);

            _mScrollbar.scrollSize = Mathf.Max(10f, scrollSize);
            _mScrollbar.minValue = 0f;
            _mScrollbar.maxValue = height;
            _mScrollbar.incrementAmount = Mathf.Max(1f, amount);
            UpdateScrollPosition();
        }

        private void UpdateScrollPosition()
        {
            if (_mLock || _mRowHeight <= 0) return;

            _mLock = true;

            float pos = _mPos * (height - _mScrollbar.scrollSize) / (_mRowsData.m_size - height / _mRowHeight);
            if (pos != _mScrollbar.value)
                _mScrollbar.value = pos;

            _mLock = false;
        }


        private void SetupControls()
        {
            if (_mPanel != null) return;

            // Panel 
            _mPanel = AddUIComponent<UIPanel>();
            _mPanel.size = size;
            _mPanel.backgroundSprite = _mBackgroundSprite;
            _mPanel.color = _mColor;
            _mPanel.clipChildren = true;
            _mPanel.relativePosition = Vector2.zero;

            // Scrollbar
            _mScrollbar = AddUIComponent<UIScrollbar>();
            _mScrollbar.width = 20f;
            _mScrollbar.height = height;
            _mScrollbar.orientation = UIOrientation.Vertical;
            _mScrollbar.pivot = UIPivotPoint.BottomLeft;
            _mScrollbar.AlignTo(this, UIAlignAnchor.TopRight);
            _mScrollbar.minValue = 0;
            _mScrollbar.value = 0;
            _mScrollbar.incrementAmount = 50;

            UISlicedSprite tracSprite = _mScrollbar.AddUIComponent<UISlicedSprite>();
            tracSprite.relativePosition = Vector2.zero;
            tracSprite.autoSize = true;
            tracSprite.size = tracSprite.parent.size;
            tracSprite.fillDirection = UIFillDirection.Vertical;
            tracSprite.spriteName = "ScrollbarTrack";

            _mScrollbar.trackObject = tracSprite;

            UISlicedSprite thumbSprite = tracSprite.AddUIComponent<UISlicedSprite>();
            thumbSprite.relativePosition = Vector2.zero;
            thumbSprite.fillDirection = UIFillDirection.Vertical;
            thumbSprite.autoSize = true;
            thumbSprite.width = thumbSprite.parent.width - 8;
            thumbSprite.spriteName = "ScrollbarThumb";

            _mScrollbar.thumbObject = thumbSprite;

            // Rows
            CheckRows();

            _mScrollbar.eventValueChanged += (c, t) =>
            {
                if (_mLock || _mRowHeight <= 0) return;

                _mLock = true;

                ListPosition = _mScrollbar.value * (_mRowsData.m_size - height / _mRowHeight) / (height - _mScrollbar.scrollSize - 1f);
                _mLock = false;
            };
        }
        #endregion
    }
}
