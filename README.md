# GAssetBundleManager

To use Unity AssetBundle, you need to deal with download API, Cache API, dependency resolution. (too complicated!)

GAssetBundleManager detects updates of the AssetBundle on the web server, downloads the AssetBundle, and overwrites previous Cache.

Of course, if the latest Cache is stored, the AssetBundle is read from Cache.

# How to use

Unity 5.6.x or later.

import [UniRx](https://github.com/neuecc/UniRx) unitypackage

import GAssetBundleMangaer unitypackage from releases page.

## Build and Deploy AssetBundle

select `Assets/GAssetBundle/Build Window`.

select `BuildTarget` and Compression Option

click `Build AssetBundle` button.

AssetBundles are created in `Assets/AssetBundles`.

copy `AssetBundles` folder to the web server.

## Load AssetBundle Sample

```:Sample.cs
using System;
using System.Collections;
using UnityEngine;
using UniRx;
using GAssetBundle.Web;

namespace GAssetBundle.Sample
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

            var assetBundleName = "cube";
            var assetName = "Cube";
            var yieldDownloadCube = assetBundleManager.GetAsset<GameObject>(assetBundleName, assetName, new ConsoleProgress()).ToYieldInstruction();
            yield return yieldDownloadCube;
            Instantiate(yieldDownloadCube.Result);

            Debug.Log("CacheSize: " + AssetBundleCache.GetCacheSize());
        }
    }
}
```
## License
MIT

