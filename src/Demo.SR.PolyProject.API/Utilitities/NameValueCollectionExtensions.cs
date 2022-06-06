using System.Collections.Generic;
using System.Collections.Specialized;

namespace Demo.SR.PolyProject.API.Utilities
{
    internal static class NameValueCollectionExtensions
    {
        public static bool TryGetValue(this NameValueCollection collection, string key, out IEnumerable<string> values)
        {
            values = collection.GetValues(key);

            return values != null;
        }
    }
}
