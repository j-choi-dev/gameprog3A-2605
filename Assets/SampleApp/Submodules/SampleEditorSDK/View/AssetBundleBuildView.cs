using Cysharp.Threading.Tasks;
using SampleEditorSDK.Application;
using SampleEditorSDK.Domain;
using System;
using UnityEditor;
using UnityEngine;

namespace SampleEditorSDK.View
{
    public static class AssetBundleBuildView
    {
        // Start is called before the first frame update
        public static async UniTask<bool> ExecuteAssetBundleBuild( string version )
        {
            var platform = EditorUserBuildSettings.activeBuildTarget.ToString();
            var isSuccessParse = Enum.TryParse<BuildTarget>(platform, out var target);
            if(!isSuccessParse || !Enum.IsDefined( typeof( BuildTarget ), target ))
            {
                throw new Exception( $"Invalid Platform :: {target}" );
            }

            var targetGroup = default(BuildTargetGroup);
            IAssetBundleBuildDomain domain = null;
            switch(target)
            {

                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows:
                    targetGroup = BuildTargetGroup.Standalone;
                    domain = new StandaloneAssetBundleBuilder();
                    break;

                case BuildTarget.Android:
                    throw new NotSupportedException($"AssetBundle build is not implemented for target: {target}");
                    targetGroup = BuildTargetGroup.Android;
                    break;

                case BuildTarget.iOS:
                    throw new NotSupportedException($"AssetBundle build is not implemented for target: {target}");
                    targetGroup = BuildTargetGroup.iOS;
                    break;

                default:
                    throw new Exception( $"Invalid Platform :: {target}" );
            }

            var buildInfo = new AssetBundleBuildInfo();
            buildInfo.Platform = platform.ToString();
            buildInfo.BuildTarget = targetGroup.ToString();
            buildInfo.Version = version;
            var application = new AssetBundleBuildApplication(domain);
            var result = await application.ExecuteAssetBundleBuild(buildInfo);
            if(result == false)
            {
                throw new Exception( "AssetBundle Build Failed" );
            }
            Debug.Log( "AssetBundle Build Success" );
            return true;
        }
    }
}
