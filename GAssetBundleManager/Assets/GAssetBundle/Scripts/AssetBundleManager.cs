using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;

namespace GAssetBundle
{
    public class AssetBundleManager
    {
        public static AssetBundleManager Instance = new AssetBundleManager();

        public AssetBundleManifest Manifest { get; private set; }
        string assetBundleUrl;
        Dictionary<string, LoadedAssetBundle> loadedAssetBundleMap = new Dictionary<string, LoadedAssetBundle>();

        AssetBundleManager()
        {
        }

        public IObservable<Unit> ConnectToServer(string assetBundleUrl)
        {
            var platformName = GetPlatformName();
            this.assetBundleUrl = assetBundleUrl + platformName + "/";
            return DownloadAssetBundleManifest(platformName);
        }

        public IObservable<AssetBundle> GetAssetBundle(string assetBundleName, IProgress<float> downloadProgress = null)
        {
            var progress = downloadProgress ?? EmptyProgress.Default;
            return Observable.FromCoroutine(_ => DownloadAndCacheAllDependenciesAsBinary(assetBundleName, progress))
                             .SelectMany(_ => LoadDependencies(assetBundleName))
                             .Select(_ => loadedAssetBundleMap[assetBundleName])
                             .Select(lab => lab.AssetBundle);
        }

        public IObservable<Object[]> GetAllAssets(string assetBundleName, IProgress<float> downloadProgress = null)
        {
            return GetAssetBundle(assetBundleName, downloadProgress)
                .SelectMany(ab => ab.GetAllAssetNames().Select(n => LoadAssetFromAssetBundle<Object>(ab, n)).WhenAll());
        }

        public IObservable<T> GetAsset<T>(string assetBundleName, string assetName, IProgress<float> downloadProgress = null) where T : UnityEngine.Object
        {
            return GetAssetBundle(assetBundleName, downloadProgress)
                .SelectMany(ab => LoadAssetFromAssetBundle<T>(ab, assetName));
        }

        public void UnloadAssetBundle(string assetBundleName)
        {
            var dependencies = Manifest.GetDirectDependencies(assetBundleName);
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundle(dependency);
            }
            LoadedAssetBundle loadedAssetBundle;
            if (loadedAssetBundleMap.TryGetValue(assetBundleName, out loadedAssetBundle))
            {
                loadedAssetBundle.RefCount -= 1;
                if (loadedAssetBundle.RefCount <= 0)
                {
                    loadedAssetBundle.AssetBundle.Unload(false);
                    loadedAssetBundleMap.Remove(assetBundleName);
                }
            }
        }

        public void UnloadAllAssetBundle()
        {
            foreach (var loadedAssetBundle in loadedAssetBundleMap.Values)
            {
                loadedAssetBundle.AssetBundle.Unload(false);
            }
            loadedAssetBundleMap.Clear();
        }

        // BuildTarget#ToString()
        static string GetPlatformName()
        {
#if UNITY_IOS
            return "iOS";
#elif UNITY_ANDROID
            return "Android";
#else
            throw new System.NotSupportedException();
#endif
        }

        IObservable<Unit> DownloadAssetBundleManifest(string assetBundleName)
        {
            return DownloadAssetBundle(assetBundleName).SelectMany(ab =>
            {
                var observable = LoadAssetFromAssetBundle<AssetBundleManifest>(ab, "AssetBundleManifest");
                return observable.Finally(() => ab.Unload(false));
            })
                                                       .Do(m => Manifest = m)
                                                       .AsUnitObservable();
        }

        IObservable<AssetBundle> DownloadAssetBundle(string assetBundleName)
        {
            return ObservableWeb.GetAssetBundle(assetBundleUrl + assetBundleName);
        }

        IObservable<T> LoadAssetFromAssetBundle<T>(AssetBundle assetBundle, string assetName) where T : UnityEngine.Object
        {
            return assetBundle.LoadAssetAsync<T>(assetName)
                              .AsAsyncOperationObservable()
                              .Select(a => a.asset as T);
        }

        IEnumerator DownloadAndCacheAllDependenciesAsBinary(string assetBundleName, IProgress<float> downloadProgress)
        {
            var dependencies = Manifest.GetAllDependencies(assetBundleName);
            var allAssetBundles = dependencies.Concat(new[] { assetBundleName });
            var needDownloads = allAssetBundles.Where(ab => !AssetBundleCache.IsCached(ab, Manifest.GetAssetBundleHash(ab))).ToArray();

            var abdProgress = new AssetBundleDownloadProgress(downloadProgress, needDownloads.Length);

            foreach (var ab in needDownloads)
            {
                yield return DownloadAndCacheAsBinary(ab, abdProgress).ToYieldInstruction();
                abdProgress.Next();
            }
            abdProgress.Complete();
        }

        IEnumerator DownloadAndCacheAsBinary(string assetBundleName, IProgress<float> progress)
        {
            var url = assetBundleUrl + assetBundleName;
            var getBytes = ObservableWeb.GetAndGetBytes(url, progress).ToYieldInstruction();
            yield return getBytes;
            var hash = Manifest.GetAssetBundleHash(assetBundleName);
            AssetBundleCache.Save(assetBundleName, hash, getBytes.Result);
        }

        IEnumerator LoadDependencies(string assetBundleName)
        {
            var dependencies = Manifest.GetDirectDependencies(assetBundleName);
            foreach (var dependency in dependencies)
            {
                var download = LoadDependencies(dependency).ToYieldInstruction();
                yield return download;
            }

            LoadedAssetBundle loadedAssetBundle;
            if (loadedAssetBundleMap.TryGetValue(assetBundleName, out loadedAssetBundle))
            {
                loadedAssetBundle.RefCount += 1;
                yield break;
            }

            var loading = AssetBundleCache.Load(assetBundleName).ToYieldInstruction();
            yield return loading;
            loadedAssetBundleMap.Add(assetBundleName, new LoadedAssetBundle(loading.Result));
        }
    }
    public class AssetBundleDownloadProgress : IProgress<float>
    {
        readonly IProgress<float> progress;
        readonly float progressPerCount;
        int current;

        public AssetBundleDownloadProgress(IProgress<float> progress, int totalCount)
        {
            this.progress = progress;
            current = 0;
            progressPerCount = 1 / (float)totalCount;
        }

        public void Next()
        {
            current += 1;
            Report(0);
        }

        public void Complete()
        {
            progress.Report(1);
        }

        public void Report(float value)
        {
            if (value < 0)
                return;
            var currentProgress = progressPerCount * (current + value);
            progress.Report(currentProgress);
        }
    }

    class EmptyProgress : IProgress<float>
    {
        EmptyProgress() { }

        public static EmptyProgress Default = new EmptyProgress();

        public void Report(float value)
        {
        }
    }
}