# GAssetBundleManager

To use Unity AssetBundle, you need to deal with download API, Cache API, dependency resolution. (too complicated!)

GAssetBundleManager detects updates of the AssetBundle on the web server, downloads the AssetBundle, and overwrites previous Cache.

Of course, if the latest Cache is stored, the AssetBundle is read from Cache.

# How to use

Unity 5.6.x or later.

import custom UniRx unitypackage (only add `ObservableWeb.cs` and `ObservableWebJson.cs`)
[UniRx-UnityWebRequest-AsObservable-#101](https://github.com/kado-yasuyuki/UniRx/releases/tag/5.6.0)

import GAssetBundleMangaer unitypackage from releases page.

## Build and Deploy AssetBundle

select `Assets/GAssetBundle/Build Window`.

select `BuildTarget` and Compression Option

click `Build AssetBundle` button.

AssetBundles are created in `Assets/AssetBundles`.

copy `AssetBundles` folder to the web server.

## Load AssetBundle Sample

```
using System;
using System.Collections;
using UnityEngine;
using UniRx;

namespace GAssetBundle
{
    public class Sample : MonoBehaviour
    {
        IEnumerator Start()
        {
            var assetBundleUrl = "http://localhost:8080/AssetBundles/";
            var assetBundleManager = AssetBundleManager.Instance;
            var yieldConnectToServer = assetBundleManager.ConnectToServer(assetBundleUrl)
                                              .OnErrorRetry<Unit, UnityWebRequestErrorException>(e => Debug.LogError(e.Message), TimeSpan.FromSeconds(5))
                                              .ToYieldInstruction();

            yield return yieldConnectToServer;

            Debug.Log("CacheSize: " + AssetBundleCache.GetCacheSize());

            var assetBundleName = "masterdata";
            var assetName = "masterdata";
            var yieldMasterdata = assetBundleManager.GetAsset<MasterData>(assetBundleName, assetName, new ConsoleProgress()).ToYieldInstruction();
            yield return yieldMasterdata;
            var masterData = yieldMasterdata.Result;
            Debug.Log("MasterData version: " + masterData.version);

            Debug.Log("CacheSize: " + AssetBundleCache.GetCacheSize());
        }

        class ConsoleProgress : IProgress<float>
        {
            public void Report(float value)
            {
                Debug.Log(value.ToString("f3"));
            }
        }
    }
}

```