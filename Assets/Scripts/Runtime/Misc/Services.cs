using System;
using System.Collections.Generic;

namespace Runner.Core
{
    public static class Services //much safe
    {
        static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        public static T Get<T>() => (T) services[typeof(T)];

        public static void Register<T>(T service) => services.Add(typeof(T), service);
        public static void Unregister<T>() => services.Remove(typeof(T));
    }
}
