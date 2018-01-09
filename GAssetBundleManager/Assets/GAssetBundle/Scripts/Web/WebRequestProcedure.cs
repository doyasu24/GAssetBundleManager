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

        public class UnityWebRequestErrorException : Exception
        {
            public string RawErrorMessage { get; private set; }
            public bool HasResponse { get; private set; }
            public string Text { get; private set; }
            public System.Net.HttpStatusCode StatusCode { get; private set; }
            public Hash ResponseHeaders { get; private set; }
            public UnityWebRequest WWW { get; private set; }

            // cache the text because if www was disposed, can't access it.
            public UnityWebRequestErrorException(UnityWebRequest www, string text = "")
            {
                this.WWW = www;
                this.RawErrorMessage = www.error;
                this.ResponseHeaders = www.GetResponseHeaders();
                this.HasResponse = false;
                this.Text = text;

                var splitted = RawErrorMessage.Split(' ', ':');
                if (splitted.Length != 0)
                {
                    int statusCode;
                    if (int.TryParse(splitted[0], out statusCode))
                    {
                        this.HasResponse = true;
                        this.StatusCode = (System.Net.HttpStatusCode)statusCode;
                    }
                }
            }

            public override string Message
            {
                get
                {
                    return ToString();
                }
            }

            public override string ToString()
            {
                var text = this.Text;
                if (string.IsNullOrEmpty(text))
                {
                    return RawErrorMessage;
                }
                else
                {
                    return RawErrorMessage + " " + text;
                }
            }

        }
    }
}