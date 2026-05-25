using Cysharp.Threading.Tasks;
using UnityEditor;

namespace SampleEditorSDK.Domain
{
    public interface IAssetBundleBuildDomain
    {
        UniTask<bool> PreProcess(AssetBundleBuildInfo platform );
        UniTask<bool> BuildProcess();
        UniTask<bool> PostProcess();
    }
}
