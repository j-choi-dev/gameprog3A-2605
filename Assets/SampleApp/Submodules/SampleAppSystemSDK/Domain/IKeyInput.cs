using System;
using UnityEngine;

namespace SampleAppSystemSDK.Domain
{
    public interface IKeyInput
    {
        IObservable<KeyCode> OnKeyCodeInput { get; }
    }
}