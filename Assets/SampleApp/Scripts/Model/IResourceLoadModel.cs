using Cysharp.Threading.Tasks;
using SampleResourceSDK.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SampleApp.Model
{
    public interface IResourceLoadModel
    {
        IObservable<IReadOnlyList<string>> OnResourceListChanged { get; }
        UniTask<bool> InitializeProcess();
        UniTask<bool> LoadResourceProcess( string id );
    }
}
