using ColossalFramework.UI;
using UnityEngine;

namespace EnhancedBuildPanel.GUI_09
{
    public class UITitleBar : UIPanel
    {
        private UIDragHandle m_drag;
        private UISprite m_icon;
        private UILabel m_title;

        public string iconSprite
        {
            get { return m_icon.spriteName; }
            set
            {
                if (m_icon == null) return;
                m_icon.atlas = UIUtils.defaultAtlas;
                m_icon.spriteName = value;

                if (m_icon.spriteInfo != null)
                {
                    m_icon.size = m_icon.spriteInfo.pixelSize;
                    UIUtils.ResizeIcon(m_icon, new Vector2(32, 32));
                    m_icon.relativePosition = new Vector3(10, 5);
                }
            }
        }

        public UIButton closeButton { get; private set; }

        public string title
        {
            get { return m_title.text; }
            set { m_title.text = value; }
        }

        public override void Awake()
        {
            base.Awake();

            m_icon = AddUIComponent<UISprite>();
            m_title = AddUIComponent<UILabel>();
            closeButton = AddUIComponent<UIButton>();
            m_drag = AddUIComponent<UIDragHandle>();

            height = 40;
            width = 450;
            title = "(None)";
            iconSprite = "";
        }

        public override void Start()
        {
            base.Start();

            width = parent.width;
            relativePosition = Vector3.zero;
            isVisible = true;
            canFocus = true;
            isInteractive = true;

            m_drag.width = width - 50;
            m_drag.height = height;
            m_drag.relativePosition = Vector3.zero;
            m_drag.target = parent;

            m_icon.spriteName = iconSprite;
            m_icon.relativePosition = new Vector3(10, 5);

            m_title.relativePosition = new Vector3(50, 13);
            m_title.text = title;

            closeButton.atlas = UIUtils.defaultAtlas;
            closeButton.relativePosition = new Vector3(width - 35, 2);
            closeButton.normalBgSprite = "buttonclose";
            closeButton.hoveredBgSprite = "buttonclosehover";
            closeButton.pressedBgSprite = "buttonclosepressed";
            closeButton.eventClick += (component, param) => parent.Hide();
        }
    }
}