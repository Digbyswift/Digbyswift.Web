#if NETSTANDARD2_1
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Digbyswift.Web.Mvc.Extensions
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            return tempData.TryGetValue(key, out var o) && o != null
                ? JsonConvert.DeserializeObject<T>((string)o)
                : null;
        }
    }
}
#endif