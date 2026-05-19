using Cysharp.Threading.Tasks;
using SampleCharacterSDK.Domain;

namespace SampleResourceSDK.Application
{
    public interface IBundleResourceApplication
    {
        bool GetIsExist( string fileName );
        UniTask<ICharacter> LoadCharacterObject( string fileName );
    }
}
