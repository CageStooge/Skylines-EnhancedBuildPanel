using System;

using UnityEngine;
using ColossalFramework.UI;
using ColossalFramework.Steamworks;
using ColossalFramework.Globalization;

namespace EnhancedBuildPanel.UI
{
    public class UIPrefabItem : UIPanel, IUIFastListRow
    {
        public void Display(object data, bool isRowOdd)
        {
            throw new NotImplementedException();
        }

        public void Select(bool isRowOdd)
        {
            throw new NotImplementedException();
        }

        public void Deselect(bool isRowOdd)
        {
            throw new NotImplementedException();
        }

        public bool Enabled { get; set; }
        public Vector3 RelativePosition { get; set; }
        public event MouseEventHandler EventClick;
    }
}