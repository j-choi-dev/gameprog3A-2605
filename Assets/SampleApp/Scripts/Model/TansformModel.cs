using SampleAppSystemSDK.Application;
using SampleResourceSDK.Application;
using System;
using UniRx;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace SampleApp.Model
{
    public class TansformModel : ITansformModel, IDisposable
    {
        private IKeyInputApplication _keyInputApplication;
        private IBundleResourceApplication _bundlResourceApplication;

        private Subject<Vector3> _onPositionChanged = new Subject<Vector3>(); 
        public IObservable<Vector3> OnPositionChanged => _onPositionChanged;

        private CompositeDisposable _disposable;

        public TansformModel(IKeyInputApplication keyInput,
            IBundleResourceApplication bundlResourceApplication)
        {
            _disposable = new CompositeDisposable();
            _keyInputApplication = keyInput;
            _bundlResourceApplication = bundlResourceApplication;

            _keyInputApplication.OnKeyCodeInput
                .Subscribe(key => KeyChange(key) )
                .AddTo(_disposable);

            _bundlResourceApplication.OnCharacterLoad
                .Subscribe( arg => _onPositionChanged.OnNext(arg.Transform.localPosition) )
                .AddTo( _disposable);
        }

        private void KeyChange(KeyCode code)
        {
            Debug.Log($"keyCode = {code}");
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _disposable = null;
        }
    }
}
