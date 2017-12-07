using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace GAssetBundle
{
    public class AssetBundleBuilder : EditorWindow
    {
        static string assetBundleDirectory = "Assets/AssetBundles";

        bool isBuildiOS = false;
        bool isBuildAndroid = false;

        bool isLZMA = true;
        bool isLZ4 = false;
        bool isUncompressed = false;

        [MenuItem("Assets/GAssetBundle/Build Window")]
        static void Open()
        {
            var window = GetWindow<AssetBundleBuilder>();
            window.Show();
        }

        void OnGUI()
        {
            GUILayout.Label("BuildTarget", EditorStyles.boldLabel);
            isBuildiOS = EditorGUILayout.Toggle("iOS", isBuildiOS);
            isBuildAndroid = EditorGUILayout.Toggle("Android", isBuildAndroid);


            GUILayout.Label("Compression Option", EditorStyles.boldLabel);

            if (EditorGUILayout.Toggle("LZMA (Default)", isLZMA))
            {
                isLZMA = true;
                isLZ4 = false;
                isUncompressed = false;
            }

            if(EditorGUILayout.Toggle("LZ4 (ChunkBased)", isLZ4))
            {
                isLZMA = false;
                isLZ4 = true;
                isUncompressed = false;
            }

            if (EditorGUILayout.Toggle("Uncompressed", isUncompressed))
            {
                isLZMA = false;
                isLZ4 = false;
                isUncompressed = true;
            }

            if(GUILayout.Button("Build AssetBundle"))
            {
                BuildAllAssetBundles(GetBuildTargets(), GetCompressionOption());
            }
        }

        BuildAssetBundleOptions GetCompressionOption()
        {
            if (isLZ4)
                return BuildAssetBundleOptions.ChunkBasedCompression;
            if (isUncompressed)
                return BuildAssetBundleOptions.UncompressedAssetBundle;
            return BuildAssetBundleOptions.None;
        }

        IEnumerable<BuildTarget> GetBuildTargets()
        {
            var targets = new List<BuildTarget>();
            if (isBuildiOS)
                targets.Add(BuildTarget.iOS);
            if (isBuildAndroid)
                targets.Add(BuildTarget.Android);

            return targets;
        }

        static void BuildAllAssetBundles(IEnumerable<BuildTarget> targets, BuildAssetBundleOptions options)
        {
            CreateDirectoryIfNotExists(assetBundleDirectory);

            foreach(var target in targets)
            {
                BuildAssetBundle(target, options);
                Debug.Log("AssetBundle Build Finished: " + target);
            }
        }

        static void BuildAssetBundle(BuildTarget buildTarget, BuildAssetBundleOptions options)
        {
            var path = Path.Combine(assetBundleDirectory, buildTarget.ToString());
            CreateDirectoryIfNotExists(path);
            BuildPipeline.BuildAssetBundles(path, options, buildTarget);
        }

        static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
