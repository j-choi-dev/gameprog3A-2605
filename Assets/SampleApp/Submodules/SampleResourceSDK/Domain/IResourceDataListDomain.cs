using System;
using System.Collections.Generic;

namespace SampleResourceSDK.Domain
{
    public interface IResourceDataListDomain
    {
        public IReadOnlyList<ResourceDataInfo> DataInfoList { get; }
        public IObservable<IReadOnlyList<ResourceDataInfo>> OnChangedList { get; }
        bool AddIList( IReadOnlyList<ResourceDataInfo> list );
        bool AddItem( ResourceDataInfo data );
        bool RemoveItem( ResourceDataInfo data );
    }
}
