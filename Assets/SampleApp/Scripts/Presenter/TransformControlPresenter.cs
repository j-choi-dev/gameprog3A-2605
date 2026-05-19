using Cysharp.Threading.Tasks;
using SampleApp.Model;
using UnityEngine;
using Zenject;
using UniRx;

namespace SampleApp.Presenter
{
    public class TransformControlPresenter : MonoBehaviour
    {
        private ISampleAppUIView _view;
        private ITransformModel _transformModel;


        [Inject]
        public void Initialize(ISampleAppUIView view,
            ITransformModel transformModel)
        {
            _view = view;
            _transformModel = transformModel;
        }

        private async void Awake()
        {
            SubscribeModel();
        }

        private void SubscribeModel()
        {
            _transformModel.OnPositionChanged
                .Subscribe(vec => _view.SetVector3(vec))
                .AddTo(this);
        }
    }
}
