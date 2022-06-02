using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

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
                return JsonSerializer.Deserialize<T>(workingValue);
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
            
            string serializedValue = JsonSerializer.Serialize(value);
            
            session.SetString(key, serializedValue);
        }

    }
}