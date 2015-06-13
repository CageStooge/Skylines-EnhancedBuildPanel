using ColossalFramework.UI;
using UnityEngine;

namespace EnhancedBuildPanel.UI
{
    public class UITitleBar : UIPanel
    {
        private UISprite _icon;
        private UILabel _title;
        private UIButton _close;
        private UIDragHandle _drag;

        public string IconSprite
        {
            get { return _icon.spriteName; }
            set
            {
                if (_icon == null) return;
                _icon.spriteName = value;

                if (_icon.spriteInfo != null)
                {
                    _icon.size = _icon.spriteInfo.pixelSize;
                    UIUtils.ResizeIcon(_icon, new Vector2(32, 32));
                    _icon.relativePosition = new Vector3(10, 5);
                }
            }
        }

        public UIButton CloseButton
        {
            get { return _close; }
        }

        public string Title
        {
            get { return _title.text; }
            set { _title.text = value; }
        }

        public override void Awake()
        {
            base.Awake();

            _icon = AddUIComponent<UISprite>();
            _title = AddUIComponent<UILabel>();
            _close = AddUIComponent<UIButton>();
            _drag = AddUIComponent<UIDragHandle>();

            height = 40;
            width = 450;
            Title = "(None)";
            IconSprite = "";
        }

        public override void Start()
        {
            base.Start();

            width = parent.width;
            relativePosition = Vector3.zero;
            isVisible = true;
            canFocus = true;
            isInteractive = true;

            _drag.width = width - 50;
            _drag.height = height;
            _drag.relativePosition = Vector3.zero;
            _drag.target = parent;

            _icon.spriteName = IconSprite;
            _icon.relativePosition = new Vector3(10, 5);

            _title.relativePosition = new Vector3(50, 13);
            _title.text = Title;

            _close.relativePosition = new Vector3(width - 35, 2);
            _close.normalBgSprite = "buttonclose";
            _close.hoveredBgSprite = "buttonclosehover";
            _close.pressedBgSprite = "buttonclosepressed";
            _close.eventClick += (component, param) => parent.Hide();
        }
    }
}