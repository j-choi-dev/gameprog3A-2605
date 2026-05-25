using SampleAppSystemSDK.Domain;
using System;
using UniRx;
using UnityEngine;

namespace SampleAppSystemSDK.Infrastructure
{
    public class KeyInputInfrastructure : MonoBehaviour, IKeyInput
    {
        private Subject<KeyCode> _onKeyCodeInput = new Subject<KeyCode>();
        public IObservable<KeyCode> OnKeyCodeInput => _onKeyCodeInput;

        void Update()
        {
            if (Input.anyKeyDown)
            {
                foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        _onKeyCodeInput.OnNext(keyCode);
                        break;
                    }
                }
            }
        }

    }
}
