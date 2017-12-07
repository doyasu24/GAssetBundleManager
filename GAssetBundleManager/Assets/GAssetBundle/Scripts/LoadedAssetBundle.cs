using UnityEngine;

namespace GAssetBundle
{
    public class LoadedAssetBundle
    {
        public AssetBundle AssetBundle { get; private set; }
        public int RefCount { get; set; }

        public LoadedAssetBundle(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;
            RefCount = 1;
        }
    }
}