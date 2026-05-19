using System.Collections.Generic;

namespace SampleAppSystemSDK.Application
{
    public interface IParseApplication
    {
        IReadOnlyList<T> ParsingProcess<T>( string rawData ) where T : new();
    }
}
