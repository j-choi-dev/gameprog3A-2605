using Cysharp.Threading.Tasks;
using SampleEditorSDK.Domain;

namespace SampleEditorSDK.Application
{
    public class AssetBundleBuildApplication
    {
        private IAssetBundleBuildDomain _domain;
        public AssetBundleBuildApplication( IAssetBundleBuildDomain domain )
        {
            _domain = domain;
        }

        public async UniTask<bool> ExecuteAssetBundleBuild( AssetBundleBuildInfo buildInfo )
        {
            var result = await _domain.PreProcess(buildInfo);
            if(result == false)
            {
                return false;
            }
            result = await _domain.BuildProcess();
            if(result == false)
            {
                return false;
            }
            return await _domain.PostProcess();
        }
    }
}
