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