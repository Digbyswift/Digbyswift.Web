using System.Text.Json;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Digbyswift.Web.Mvc.Extensions
{
    public static class TempDataExtensions
    {
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
        {
            tempData[key] = JsonSerializer.Serialize(value);
        }

        public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
        {
            return tempData.TryGetValue(key, out var o) && o != null
                ? JsonSerializer.Deserialize<T>((string)o)
                : null;
        }
    }
}