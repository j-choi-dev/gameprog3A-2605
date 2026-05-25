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
        private ITansformModel _transformModel;


        [Inject]
        public void Initialize(ISampleAppUIView view,
            ITansformModel transformModel)
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
