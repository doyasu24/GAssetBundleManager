using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace GAssetBundle.Web
{
    public class UnityWebRequestErrorException : Exception
    {
        public string RawErrorMessage { get; private set; }
        public bool HasResponse { get; private set; }
        public string Text { get; private set; }
        public System.Net.HttpStatusCode StatusCode { get; private set; }
        public Dictionary<string, string> ResponseHeaders { get; private set; }
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