using System;
using System.Collections.Generic;

namespace SampleApp.Presenter
{
    public interface ISampleAppUIView
    {
        IObservable<int> OnSelectIndexChange { get; }
        IObservable<string> OnSelectNameChange { get; }
        void SetOptions( IReadOnlyList<string> options );
    }
}
