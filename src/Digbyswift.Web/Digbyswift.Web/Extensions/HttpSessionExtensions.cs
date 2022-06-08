#if NETSTANDARD2_1
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Digbyswift.Web.Extensions
{
    public static class HttpSessionExtensions
    {
        public static T Get<T>(this ISession session, string key)
        {
            if (!session.Keys.Contains(key))
                return default;

            var workingValue = session.GetString(key);
            if (String.IsNullOrWhiteSpace(workingValue))
                return default;

            try
            {
                return JsonConvert.DeserializeObject<T>(workingValue);
            }
            catch
            {
                return default;
            }
        }

        public static void Set<T>(this ISession session, string key, T value)
        {
            if (value == null)
                return;
            
            string serializedValue = JsonConvert.SerializeObject(value);
            
            session.SetString(key, serializedValue);
        }
    }
}
#endif
