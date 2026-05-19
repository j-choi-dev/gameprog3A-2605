using Cysharp.Threading.Tasks;
using SampleCharacterSDK.Domain;
using SampleResourceSDK.Domain;
using System;
using UniRx;

namespace SampleResourceSDK.Application
{
    public class BundleResourceApplication : IBundleResourceApplication
    {
        private IBundleResourceDomain _bundleDomain;
        private IResourceFactoryDomain _factoryDomain;

        private Subject<ICharacter> _onCharacterLoad = new Subject<ICharacter>();
        public IObservable<ICharacter> OnCharacterLoad => _onCharacterLoad;

        public BundleResourceApplication( IBundleResourceDomain bundleDomain, 
            IResourceFactoryDomain factoryDomain )
        {
            _bundleDomain = bundleDomain;
            _factoryDomain = factoryDomain;
        }

        public bool GetIsExist( string fileName )
            => _bundleDomain.GetIsExist( fileName );

        public async UniTask<ICharacter> LoadCharacterObject( string fileName )
        {
            var obj = await _bundleDomain.LoadObject( fileName );
            var character = await _factoryDomain.GenerateCharacter(obj);
            _onCharacterLoad.OnNext( character );
            return character;
        }
    }
}
