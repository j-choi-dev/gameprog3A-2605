using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace SampleApp.Model
{
    public interface IResourceLoadModel
    {
        IObservable<IReadOnlyList<string>> OnResourceListChanged { get; }
        IObservable<bool> OnIsResourceLaad {  get; }
        UniTask<bool> InitializeProcess();
        UniTask<bool> LoadResourceProcess( string id );
    }
}
