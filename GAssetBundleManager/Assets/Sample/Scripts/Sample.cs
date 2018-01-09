using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace GAssetBundle
{
    public class Sample : MonoBehaviour
    {
        [SerializeField]
        string assetBundleServerUrl;

        void Start()
        {
            AssetBundleManager.Instance.ConnectToServer(assetBundleServerUrl).Subscribe(_ => OnConnected());
        }

        void OnConnected()
        {
            AssetBundleManager.Instance.GetAsset<GameObject>("cube", "Cube")
                              .Subscribe(go => Instantiate(go));

        }
    }
}