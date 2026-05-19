using SampleAppSystemSDK.Domain;
using System;
using UniRx;
using UnityEngine;

namespace SampleAppSystemSDK.Application
{
    public class KeyInputApplication : IKeyInputApplication, IDisposable
    {
        private IKeyInput _keyInput;
        public IObservable<KeyCode> OnKeyCodeInput => _keyInput.OnKeyCodeInput;

        private CompositeDisposable _disposable;

        public KeyInputApplication( IKeyInput keyInput )
        {
            _disposable = new CompositeDisposable();
            _keyInput = keyInput;
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _disposable = null;
        }
    }
}
