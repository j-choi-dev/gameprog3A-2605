using System;
using UnityEngine;

namespace SampleAppSystemSDK.Application
{
    public interface IKeyInputApplication
    {
        IObservable<KeyCode> OnKeyCodeInput { get; }
    }
}
