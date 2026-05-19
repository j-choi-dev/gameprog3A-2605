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

        private void Start()
        {
            _view.SetVector3(Vector3.zero);
            _view.SetTransformInteractable(false);
        }

        private async void SubscribeView()
        {
            _view.OnSelectNameChange
                .Subscribe( async id => await _resourceLoadModel.LoadResourceProcess( id ) )
                .AddTo( this );
        }

        private void SubscribeModel()
        {
            _resourceLoadModel.OnResourceListChanged
                .Subscribe( arg => _view.SetOptions( arg ) )
                .AddTo( this );

            _resourceLoadModel.OnIsResourceLaad
                .Subscribe(isLoad => _view.SetTransformInteractable(isLoad))
                .AddTo(this);

        }
    }
}
