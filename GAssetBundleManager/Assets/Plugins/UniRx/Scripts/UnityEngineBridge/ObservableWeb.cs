#if UNITY_5_4_OR_NEWER

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

#if !UniRxLibrary
using ObservableUnity = UniRx.Observable;
#endif

namespace UniRx
{
    using System.Threading;
    using Hash = System.Collections.Generic.Dictionary<string, string>;

    public static partial class ObservableWeb
    {
        #region GET
        public static IObservable<UnityWebRequest> GetWWW(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Get(url).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> GetWWW(string url, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Get(url), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<string> Get(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return GetWWW(url, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Get(string url, IProgress<float> downloadProgress = null)
        {
            return GetWWW(url, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> GetAndGetBytes(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return GetWWW(url, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> GetAndGetBytes(string url, IProgress<float> downloadProgress = null)
        {
            return GetWWW(url, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, uint crc, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, crc).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, uint crc, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, crc), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, uint version, uint crc, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, version, crc).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, uint version, uint crc, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, version, crc), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, Hash128 hash, uint crc, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, hash, crc).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, Hash128 hash, uint crc, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, hash, crc), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

#if UNITY_2017_1_OR_NEWER
        public static IObservable<AssetBundle> GetAssetBundle(string url, CachedAssetBundle cachedAssetBundle, uint crc, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, cachedAssetBundle, crc).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }

        public static IObservable<AssetBundle> GetAssetBundle(string url, CachedAssetBundle cachedAssetBundle, uint crc, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAssetBundle(url, cachedAssetBundle, crc), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAssetBundle)w.downloadHandler).assetBundle);
        }
#endif
        public static IObservable<Texture2D> GetTexture(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
#if UNITY_2017_1_OR_NEWER
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequestTexture.GetTexture(url).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerTexture)w.downloadHandler).texture);
#else
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetTexture(url).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerTexture)w.downloadHandler).texture);
#endif
        }

        public static IObservable<Texture2D> GetTexture(string url, IProgress<float> downloadProgress = null)
        {
#if UNITY_2017_1_OR_NEWER
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequestTexture.GetTexture(url), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerTexture)w.downloadHandler).texture);
#else
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetTexture(url), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerTexture)w.downloadHandler).texture);
#endif
        }

        public static IObservable<AudioClip> GetAudioClip(string url, AudioType type, Hash headers, IProgress<float> downloadProgress = null)
        {
#if UNITY_2017_1_OR_NEWER
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequestMultimedia.GetAudioClip(url, type).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAudioClip)w.downloadHandler).audioClip);
#else
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAudioClip(url, type).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAudioClip)w.downloadHandler).audioClip);
#endif
        }

        public static IObservable<AudioClip> GetAudioClip(string url, AudioType type, IProgress<float> downloadProgress = null)
        {
#if UNITY_2017_1_OR_NEWER
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequestMultimedia.GetAudioClip(url, type), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAudioClip)w.downloadHandler).audioClip);
#else
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.GetAudioClip(url, type), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerAudioClip)w.downloadHandler).audioClip);
#endif
        }

#if UNITY_2017_1_OR_NEWER && UNITY_STANDALONE
        public static IObservable<MovieTexture> GetMovieTexture(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequestMultimedia.GetMovieTexture(url).SetHeaders(headers), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerMovieTexture)w.downloadHandler).movieTexture);
        }

        public static IObservable<MovieTexture> GetMovieTexture(string url, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequestMultimedia.GetMovieTexture(url), observer, downloadProgress, null, cancellation))
                                  .Select(w => ((DownloadHandlerMovieTexture)w.downloadHandler).movieTexture);
        }
#endif
        #endregion

        #region POST

        public static IObservable<UnityWebRequest> PostWWW(string url, string postData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, postData).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, string postData, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, postData), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, WWWForm formData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, formData).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, WWWForm formData, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, formData), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, Hash formFields, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, formFields).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, Hash formFields, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, formFields), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, List<IMultipartFormSection> multipartFromSections, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, multipartFromSections).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PostWWW(string url, List<IMultipartFormSection> multipartFromSections, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Post(url, multipartFromSections), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<string> Post(string url, string postData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, postData, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, string postData, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, postData, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, WWWForm formData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formData, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, WWWForm formData, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formData, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, Hash formFields, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formFields, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, Hash formFields, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formFields, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, List<IMultipartFormSection> multipartFromSections, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, multipartFromSections, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Post(string url, List<IMultipartFormSection> multipartFromSections, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, multipartFromSections, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, string postData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, postData, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, string postData, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, postData, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm formData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formData, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, WWWForm formData, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formData, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, Hash formFields, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formFields, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, Hash formFields, IProgress<float> downloadProgress = null)
        {
            return PostWWW(url, formFields, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, List<IMultipartFormSection> multipartFromSections, Hash headers, IProgress<float> dowloadProgress = null)
        {
            return PostWWW(url, multipartFromSections, headers, dowloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PostAndGetBytes(string url, List<IMultipartFormSection> multipartFromSections, IProgress<float> dowloadProgress = null)
        {
            return PostWWW(url, multipartFromSections, dowloadProgress).Select(w => w.downloadHandler.data);
        }
        #endregion

        #region PUT
        public static IObservable<UnityWebRequest> PutWWW(string url, byte[] bodyData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Put(url, bodyData).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PutWWW(string url, byte[] bodyData, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Put(url, bodyData), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PutWWW(string url, string bodyData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Put(url, bodyData).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> PutWWW(string url, string bodyData, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Put(url, bodyData), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<string> Put(string url, byte[] bodyData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Put(string url, byte[] bodyData, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Put(string url, string bodyData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Put(string url, string bodyData, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> PutAndGetBytes(string url, byte[] bodyData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PutAndGetBytes(string url, byte[] bodyData, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PutAndGetBytes(string url, string bodyData, Hash headers, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> PutAndGetBytes(string url, string bodyData, IProgress<float> downloadProgress = null)
        {
            return PutWWW(url, bodyData, downloadProgress).Select(w => w.downloadHandler.data);
        }
        #endregion

        #region HEAD
        public static IObservable<UnityWebRequest> HeadWWW(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Head(url).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> HeadWWW(string url, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Head(url), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<string> Head(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return HeadWWW(url, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Head(string url, IProgress<float> downloadProgress = null)
        {
            return HeadWWW(url, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> HeadAndGetBytes(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return HeadWWW(url, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> HeadAndGetBytes(string url, IProgress<float> downloadProgress = null)
        {
            return HeadWWW(url, downloadProgress).Select(w => w.downloadHandler.data);
        }
        #endregion

        #region DELETE
        public static IObservable<UnityWebRequest> DeleteWWW(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Delete(url).SetHeaders(headers), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<UnityWebRequest> DeleteWWW(string url, IProgress<float> downloadProgress = null)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(UnityWebRequest.Delete(url), observer, downloadProgress, null, cancellation));
        }

        public static IObservable<string> Delete(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return DeleteWWW(url, headers, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<string> Delete(string url, IProgress<float> downloadProgress = null)
        {
            return DeleteWWW(url, downloadProgress).Select(w => w.downloadHandler.text);
        }

        public static IObservable<byte[]> DeleteAndGetBytes(string url, Hash headers, IProgress<float> downloadProgress = null)
        {
            return DeleteWWW(url, headers, downloadProgress).Select(w => w.downloadHandler.data);
        }

        public static IObservable<byte[]> DeleteAndGetBytes(string url, IProgress<float> downloadProgress = null)
        {
            return DeleteWWW(url, downloadProgress).Select(w => w.downloadHandler.data);
        }
        #endregion

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

        public static IObservable<UnityWebRequest> ProceedWebRequest(Func<UnityWebRequest> www, IProgress<float> downloadProgress, IProgress<float> uploadProgress)
        {
            return ObservableUnity.FromCoroutine<UnityWebRequest>((observer, cancellation) => ProceedWebRequestCoroutine(www(), observer, downloadProgress, uploadProgress, cancellation));
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

    public class UnityWebRequestErrorException : Exception
    {
        public string RawErrorMessage { get; private set; }
        public bool HasResponse { get; private set; }
        public string Text { get; private set; }
        public System.Net.HttpStatusCode StatusCode { get; private set; }
        public System.Collections.Generic.Dictionary<string, string> ResponseHeaders { get; private set; }
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

#endif