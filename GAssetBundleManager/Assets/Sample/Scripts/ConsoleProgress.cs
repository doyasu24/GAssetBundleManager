using UnityEngine;
using UniRx;

namespace GAssetBundle.Sample
{
    public class ConsoleProgress : IProgress<float>
    {
        public void Report(float value)
        {
            Debug.Log(value.ToString("f3"));
        }
    }
}