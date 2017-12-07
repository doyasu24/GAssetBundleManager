#if UNITY_5_4_OR_NEWER

using System;
using UnityEngine.Networking;

#if !UniRxLibrary
using ObservableUnity = UniRx.Observable;
#endif

namespace UniRx
{
    using Hash = System.Collections.Generic.Dictionary<string, string>;

    public static partial class ObservableWeb
    {
        const string contentTypeJson = "application/json; charset=UTF-8";

        static UnityWebRequest CreateJsonWebRequest(string url, string method, string json, Hash headers)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(json);
            var uploadHandler = new UploadHandlerRaw(bytes);
            uploadHandler.contentType = contentTypeJson;
            var www = new UnityWebRequest(url, method, new DownloadHandlerBuffer(), uploadHandler);
            return www.SetHeaders(headers);
        }

        static UnityWebRequest CreateJsonWebRequest(string url, string method, byte[] json, Hash headers)
        {
            var uploadHandler = new UploadHandlerRaw(json);
            uploadHandler.contentType = contentTypeJson;
            var www = new UnityWebRequest(url, method, new DownloadHandlerBuffer(), uploadHandler);
            return www.SetHeaders(headers);
        }

        #region POST

        public static IObservable<string> PostJson(string url, string json, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPOST, json, headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> PostJson(string url, string json, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPOST, json, null), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> PostJson(string url, byte[] json, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPOST, json, headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostJson(string url, byte[] json, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPOST, json, null), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.data);
        }

        #endregion

        #region PUT

        public static IObservable<string> PutJson(string url, string json, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPUT, json, headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> PutJson(string url, string json, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPUT, json, null), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> PutJson(string url, byte[] json, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPUT, json, headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PutJson(string url, byte[] json, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(CreateJsonWebRequest(url, UnityWebRequest.kHttpVerbPUT, json, null), observer, downloadProgress, null, cancellation))
                                  .Select(w => w.downloadHandler.data);
        }

        #endregion
    }
}
#endif