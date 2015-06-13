using System;

namespace EnhancedBuildPanel
{
    public class AssetData : IComparable
    {
        public enum Sorting
        {
            Name = 0,
            Triangles,
            LodTriangles,
            Weight,
            LodWeight,
            TextureSize,
            LodTextureSize
        }

        private PrefabInfo _prefab;

        public PrefabInfo prefab
        {
            get { return _prefab; }
            set
            {
                if (_prefab != value)
                {
                    _prefab = value;

                    Name = GetLocalizedName(_prefab);
                }
            }
        }

        public string Name;
        public string SteamId;

        public static Sorting Sorted = Sorting.Name;
        public static bool AscendingSort = true;

        public AssetData(PrefabInfo prefab)
        {
            this.prefab = prefab;
        }

        public int CompareTo(object obj)
        {
            AssetData a, b;
            if (!AscendingSort)
            {
                a = this;
                b = obj as AssetData;
            }
            else
            {
                b = this;
                a = obj as AssetData;
            }

            if (a == null || b == null) return -1;

            if (Sorted == Sorting.Name)
                return b.Name.CompareTo(a.Name);


            return 0;
        }

        private static string GetLocalizedName(PrefabInfo prefab)
        {
           string localizedName = prefab.name;
            // Removes the steam ID and trailing _Data from the name
            localizedName = localizedName.Substring(localizedName.IndexOf('.') + 1).Replace("_Data", "");

            return localizedName;
        }

        private static string GetSteamID(PrefabInfo prefab)
        {
            string steamID = null;

            if (prefab.name.Contains("."))
            {
                int id;
                steamID = prefab.name.Substring(0, prefab.name.IndexOf("."));
                if (!Int32.TryParse(steamID, out id)) return null;
            }

            return steamID;
        }




    }
}
