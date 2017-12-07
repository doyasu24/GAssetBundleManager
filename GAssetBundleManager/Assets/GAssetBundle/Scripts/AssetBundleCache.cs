using UnityEngine;
using System.IO;
using UniRx;

namespace GAssetBundle
{
    public static class AssetBundleCache
    {
        public static string AssetBundlesPath = Path.Combine(Application.temporaryCachePath, "AssetBundles");

        static AssetBundleCache()
        {
            if (!Directory.Exists(AssetBundlesPath))
            {
                Directory.CreateDirectory(AssetBundlesPath);
            }
        }

        public static bool IsCached(string assetBundleName, Hash128 hash)
        {
            var path = Path.Combine(AssetBundlesPath, assetBundleName);
            var hashPath = Path.Combine(AssetBundlesPath, assetBundleName + ".hash");

            if (File.Exists(path) && File.Exists(hashPath))
            {
                var hashString = File.ReadAllText(hashPath);
                var savedHash = Hash128.Parse(hashString);
                return hash == savedHash;
            }
            return false;
        }

        public static void Save(string assetBundleName, Hash128 hash, byte[] bytes)
        {
            var path = Path.Combine(AssetBundlesPath, assetBundleName);
            File.Delete(path);
            File.WriteAllBytes(path, bytes);

            var hashPath = Path.Combine(AssetBundlesPath, assetBundleName + ".hash");
            File.Delete(hashPath);
            File.WriteAllText(hashPath, hash.ToString());
        }

        public static IObservable<AssetBundle> Load(string assetBundleName)
        {
            var path = Path.Combine(AssetBundlesPath, assetBundleName);
            return AssetBundle.LoadFromFileAsync(path)
                              .AsAsyncOperationObservable()
                              .Select(r => r.assetBundle);
        }

        public static void ClearCache(string assetBundleName)
        {
            var path = Path.Combine(AssetBundlesPath, assetBundleName);
            File.Delete(path);

            var hashPath = Path.Combine(AssetBundlesPath, assetBundleName + ".hash");
            File.Delete(hashPath);
        }

        public static void ClearAllCache()
        {
            Directory.Delete(AssetBundlesPath, true);
            Directory.CreateDirectory(AssetBundlesPath);
        }

        public static long GetCacheSize()
        {
            var info = new DirectoryInfo(AssetBundlesPath);
            return GetDirectorySize(info);
        }

        static long GetDirectorySize(DirectoryInfo info)
        {
            long size = 0;
            foreach (var f in info.GetFiles())
                size += f.Length;
            foreach (var d in info.GetDirectories())
                size += GetDirectorySize(d);
            return size;
        }
    }
}