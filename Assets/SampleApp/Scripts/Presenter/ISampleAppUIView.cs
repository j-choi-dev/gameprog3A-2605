using System;
using System.Collections.Generic;
using UnityEngine;

namespace SampleApp.Presenter
{
    public interface ISampleAppUIView
    {
        IObservable<int> OnSelectIndexChange { get; }
        IObservable<string> OnSelectNameChange { get; }
        void SetOptions( IReadOnlyList<string> options );
        void SetTransformInteractable(bool isInteractable);
        void SetVector3(Vector3 vector);
    }
}
