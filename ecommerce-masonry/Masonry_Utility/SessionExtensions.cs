using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ecommerce_masonry.Utility
{
    // Below we have a session extension so we can store complex objects by serializing and deserializing them.
    public static class SessionExtensions // Since this is extension method, it has to be static
    {
        public static void Set<T>(this ISession session, string key, T value) 
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }        
        
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
}
