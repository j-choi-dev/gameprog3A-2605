using System;

namespace SampleResourceSDK.Application
{
    public interface IResourceLoadApplication
    {
        IObservable<bool> OnLoadComplete { get; }
    }
}
