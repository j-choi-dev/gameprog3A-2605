using Cysharp.Threading.Tasks;
using SampleApp.Model;
using UnityEngine;
using Zenject;
using UniRx;

namespace SampleApp.Presenter
{
    public class ResourceSelectPresenter : MonoBehaviour
    {
        private ISampleAppUIView _view;
        private IResourceLoadModel _resourceLoadModel;

        [Inject]
        public void Initialize( ISampleAppUIView view,
            IResourceLoadModel resourceLoadModel )
        {
            _view = view;
            _resourceLoadModel = resourceLoadModel;
        }

        private async void Awake()
        {
            SubscribeView();
            SubscribeModel();

            await _resourceLoadModel.InitializeProcess();
        }

        private async void SubscribeView()
        {
            _view.OnSelectIndexChange
                .Subscribe( arg => Debug.Log( arg ) )
                .AddTo( this );
            _view.OnSelectNameChange
                .Subscribe( async id => await _resourceLoadModel.LoadResourceProcess( id ) )
                .AddTo( this );
        }

        private void SubscribeModel()
        {
            _resourceLoadModel.OnResourceListChanged
                .Subscribe( arg => _view.SetOptions( arg ) )
                .AddTo( this );
        }
    }
}
