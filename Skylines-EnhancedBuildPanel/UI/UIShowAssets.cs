using ColossalFramework.UI;
using MeshInfo.GUI;
using UnityEngine;

namespace EnhancedBuildPanel.UI
{
    public class UIShowAssets : UIPanel, IUIFastListRow
    {
        private UIPanel _background;

        public UIPanel Background
        {
            get
            {
                if (_background == null)
                {
                    _background = AddUIComponent<UIPanel>();
                    _background.width = width;
                    _background.height = 40f;
                    _background.relativePosition = Vector2.zero;

                    _background.zOrder = 0;
                }

                return _background;
            }
        }

        public bool Enabled { get; set; }
        public Vector3 RelativePosition { get; set; }

        public void Display(object data, bool isRowOdd)
        {
            //TODO: Implement whatever array has the icos
            //TODO: Figure out how many icons per row
            //TODO: Setup onClick option. 
        }

        public void Select(bool isRowOdd)
        {
        }

        public void Deselect(bool isRowOdd)
        {
        }

        public override void Start()
        {
            base.Start();
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = 740f;
            height = 40f;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            Destroy(_background);
        }
    }
}

/*

        public void Display(object prefab, bool isRowOdd)
        {
            m_meshData = prefab as MeshData;

            if (m_meshData == null || m_name == null) return;

            m_name.text = m_meshData.name;

            m_steamID.text = (m_meshData.steamID == null) ? "" : m_meshData.steamID;
            m_steamID.isVisible = (m_meshData.steamID != null);

            m_triangles.text = (m_meshData.triangles > 0) ? m_meshData.triangles.ToString("N0") : "-";
            m_lodTriangles.text = (m_meshData.lodTriangles > 0) ? m_meshData.lodTriangles.ToString("N0") : "-";

            m_weight.text = (m_meshData.weight > 0) ? m_meshData.weight.ToString("N2") : "-";
            if (m_meshData.weight >= 200)
                m_weight.textColor = new Color32(255, 0, 0, 255);
            else if (m_meshData.weight >= 100)
                m_weight.textColor = new Color32(255, 255, 0, 255);
            else if (m_meshData.weight > 0)
                m_weight.textColor = new Color32(0, 255, 0, 255);
            else
                m_weight.textColor = new Color32(255, 255, 255, 255);

            m_lodWeight.text = (m_meshData.lodWeight > 0) ? m_meshData.lodWeight.ToString("N2") : "-";
            if (m_meshData.lodWeight >= 10)
                m_lodWeight.textColor = new Color32(255, 0, 0, 255);
            else if (m_meshData.lodWeight >= 5)
                m_lodWeight.textColor = new Color32(255, 255, 0, 255);
            else if (m_meshData.lodWeight > 0)
                m_lodWeight.textColor = new Color32(0, 255, 0, 255);
            else
                m_lodWeight.textColor = new Color32(255, 255, 255, 255);

            m_textureSize.text = (m_meshData.textureSize != Vector2.zero) ? m_meshData.textureSize.x + "x" + m_meshData.textureSize.y : "-";
            m_lodTextureSize.text = (m_meshData.lodTextureSize != Vector2.zero) ? m_meshData.lodTextureSize.x + "x" + m_meshData.lodTextureSize.y : "-";

            if (isRowOdd)
            {
                background.backgroundSprite = "UnlockingItemBackground";
                background.color = new Color32(0, 0, 0, 128);
            }
            else
                background.backgroundSprite = null;
        }


        public event MouseEventHandler EventClick;
    }
}
*/