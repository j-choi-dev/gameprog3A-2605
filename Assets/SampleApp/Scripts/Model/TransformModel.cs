using SampleAppSystemSDK.Application;
using SampleResourceSDK.Application;
using System;
using UniRx;
using UnityEngine;

namespace SampleApp.Model
{
    public class TransformModel : ITransformModel, IDisposable
    {
        private readonly IKeyInputApplication _keyInputApplication;
        private readonly IBundleResourceApplication _bundleResourceApplication;

        private readonly Subject<Vector3> _onPositionChanged = new Subject<Vector3>();
        public IObservable<Vector3> OnPositionChanged => _onPositionChanged;

        private CompositeDisposable _disposable;

        private const float MoveSpeed = 3.0f;

        public TransformModel(
            IKeyInputApplication keyInput,
            IBundleResourceApplication bundleResourceApplication )
        {
            _disposable = new CompositeDisposable();

            _keyInputApplication = keyInput;
            _bundleResourceApplication = bundleResourceApplication;

            _keyInputApplication.OnKeyCodeInput
                .Select( keyCode => ConvertKeyCodeToMoveDelta( keyCode ) )
                .Where( moveDelta => moveDelta != Vector3.zero )
                .Subscribe( moveDelta =>
                {
                    Debug.Log( $"MoveDelta = {moveDelta}" );
                    // TODO °úÁ¦¢½ @Choi 26.05.18
                } )
                .AddTo( _disposable );

            _bundleResourceApplication.OnCharacterLoad
                .Subscribe( arg =>
                {
                    if( arg == null || arg.Transform == null )
                    {
                        Debug.LogError( "CharacterLoad arg or Transform is null." );
                        return;
                    }

                    _onPositionChanged.OnNext( arg.Transform.localPosition );
                } )
                .AddTo( _disposable );
        }

        private Vector3 ConvertKeyCodeToMoveDelta( KeyCode keyCode )
        {
            Vector3 direction = ConvertKeyCodeToDirection(keyCode);

            if( direction == Vector3.zero )
                return Vector3.zero;

            return direction * MoveSpeed * Time.deltaTime;
        }

        private Vector3 ConvertKeyCodeToDirection( KeyCode keyCode )
        {
            switch( keyCode )
            {
                case KeyCode.W:
                case KeyCode.UpArrow:
                    return Vector3.forward;

                case KeyCode.S:
                case KeyCode.DownArrow:
                    return Vector3.back;

                case KeyCode.A:
                case KeyCode.LeftArrow:
                    return Vector3.left;

                case KeyCode.D:
                case KeyCode.RightArrow:
                    return Vector3.right;

                default:
                    return Vector3.zero;
            }
        }

        public void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;

            _onPositionChanged.OnCompleted();
            _onPositionChanged.Dispose();
        }
    }
}