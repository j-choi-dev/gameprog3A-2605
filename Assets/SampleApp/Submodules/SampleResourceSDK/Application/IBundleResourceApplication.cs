using Cysharp.Threading.Tasks;
using SampleCharacterSDK.Domain;
using System;

namespace SampleResourceSDK.Application
{
    public interface IBundleResourceApplication
    {
        IObservable<ICharacter> OnCharacterLoad { get; }
        bool GetIsExist( string fileName );
        UniTask<ICharacter> LoadCharacterObject( string fileName );
    }
}
