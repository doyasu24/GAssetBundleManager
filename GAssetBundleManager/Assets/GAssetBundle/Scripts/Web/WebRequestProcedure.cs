using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;

namespace GAssetBundle.Web
{
    using Hash = Dictionary<string, string>;

    public static class WebRequestProcedure
    {
        public static IObservable<AssetBundle> GetAssetBundle(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            var www = UnityWebRequest.GetAssetBundle(url).SetHeaders(headers);
            return Observable.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(www, observer, downloadProgress, null, cancellation))
                             .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<byte[]> GetAndGetBytes(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            var www = UnityWebRequest.Get(url).SetHeaders(headers);
            return Observable.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(www, observer, downloadProgress, null, cancellation))
                             .Select(w => w.downloadHandler.data);
        }

        static UnityWebRequest SetHeaders(this UnityWebRequest www, Hash headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    www.SetRequestHeader(header.Key, header.Value);
                }
            }
            return www;
        }

        static IEnumerator ProceedWebRequestCoroutine(UnityWebRequest www, IObserver<UnityWebRequest> observer, IProgress<float> downloadProgress, IProgress<float> uploadProgress, CancellationToken cancel)
        {
            using (www)
            {
#if UNITY_2017_2_OR_NEWER
                www.SendWebRequest();
#else
            www.Send();
#endif
                while (!www.isDone)
                {
                    if (cancel.IsCancellationRequested)
                    {
                        www.Abort();
                        yield break;
                    }
                    try
                    {
                        if (downloadProgress != null)
                            downloadProgress.Report(www.downloadProgress);
                        if (uploadProgress != null)
                            uploadProgress.Report(www.uploadProgress);
                    }
                    catch (Exception e)
                    {
                        observer.OnError(e);
                        www.Abort();
                        yield break;
                    }
                    yield return null;
                }

                try
                {
                    if (downloadProgress != null)
                        downloadProgress.Report(www.downloadProgress);
                    if (uploadProgress != null)
                        uploadProgress.Report(www.uploadProgress);
                }
                catch (Exception e)
                {
                    observer.OnError(e);
                    www.Abort();
                    yield break;
                }

                if (cancel.IsCancellationRequested)
                {
                    yield break;
                }

                if (!string.IsNullOrEmpty(www.error))
                {
                    observer.OnError(new UnityWebRequestErrorException(www));
                }
                else
                {
                    observer.OnNext(www);
                    observer.OnCompleted();
                }
            }
        }
    }
}