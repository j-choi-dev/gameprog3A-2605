using SampleResourceSDK.Domain;
using System;
using System.Collections.Generic;

namespace SampleResourceSDK.Application
{
    public interface IResourceDataListApplication
    {
        public IReadOnlyList<ResourceDataInfo> DataInfoList { get; }
        public IObservable<IReadOnlyList<ResourceDataInfo>> OnChangedList { get; }
        bool AddItem( ResourceDataInfo data );
        bool AddIList( IReadOnlyList<ResourceDataInfo> list );
        bool RemoveItem( ResourceDataInfo data );
    }
}
