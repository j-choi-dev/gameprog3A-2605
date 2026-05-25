using Cysharp.Threading.Tasks;
using System;
using UnityEditor;
using UnityEngine;

namespace SampleEditorSDK.View
{
    public class AssetBundleBuildMenuItem
    {
        private const string MENU_NAME_ASSETBUNDLE_ONLY = "SampleTool/AssetBundleBuild";
        [MenuItem( MENU_NAME_ASSETBUNDLE_ONLY, priority = 0 )]
        private static async UniTask<bool> AssetBundleBuildOnly()
        {
            var platform = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            return await AssetBundleBuildView.ExecuteAssetBundleBuild( platform );
        }

        public static void AssetBundleBuildOnlyByExternal()
        {
            try
            {
                var args = Environment.GetCommandLineArgs();
                var version = string.Empty;
                for (int i = 1; i < args.Length; i++)
                {
                    // '-tag'라는 문자열을 찾고 다음 인덱스 값이 있는지 확인
                    if (args[i] == "-v" && i + 1 < args.Length)
                    {
                        version = args[i + 1];
                        break;
                    }
                }
                if (string.IsNullOrEmpty(version))
                {
                    throw new Exception("Missing command line argument: -v");
                }

                var result = AssetBundleBuildView.ExecuteAssetBundleBuild(version).GetAwaiter().GetResult();
                if (result == false)
                {
                    throw new Exception("AssetBundle Build Failed");
                }

                Debug.Log($"AssetBundle Build Success. Version: {version}");
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                EditorApplication.Exit(1);
            }
        }
    }
}
