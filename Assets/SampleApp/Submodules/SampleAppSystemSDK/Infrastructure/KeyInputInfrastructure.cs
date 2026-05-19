using SampleAppSystemSDK.Domain;
using System;
using UniRx;
using UnityEngine;

namespace SampleAppSystemSDK.Infrastructure
{
    public class KeyInputInfrastructure : MonoBehaviour, IKeyInput
    {
        private readonly Subject<KeyCode> _onKeyCodeInput = new Subject<KeyCode>();
        public IObservable<KeyCode> OnKeyCodeInput => _onKeyCodeInput;

        private void Update()
        {
            PublishIfPressed( KeyCode.W );
            PublishIfPressed( KeyCode.A );
            PublishIfPressed( KeyCode.S );
            PublishIfPressed( KeyCode.D );

            PublishIfPressed( KeyCode.UpArrow );
            PublishIfPressed( KeyCode.LeftArrow );
            PublishIfPressed( KeyCode.DownArrow );
            PublishIfPressed( KeyCode.RightArrow );
        }

        private void PublishIfPressed( KeyCode keyCode )
        {
            if( Input.GetKey( keyCode ) )
            {
                _onKeyCodeInput.OnNext( keyCode );
            }
        }

        private void OnDestroy()
        {
            _onKeyCodeInput.OnCompleted();
            _onKeyCodeInput.Dispose();
        }
    }
}