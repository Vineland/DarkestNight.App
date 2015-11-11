using System;
using System.Collections.Generic;
using TinyIoC;

namespace Vineland.DarkestNight.UI.Shared
{
    public static class IoC
    {
        static TinyIoCContainer _container;

        public static bool ContainerSet()
        {
            return _container != null;
        }

        public static void SetContainer(TinyIoCContainer container)
        {
            _container = container;
        }

        public static T Get<T>() where T : class
        {
            if (_container == null)
                throw new InvalidOperationException("Could not resolve the current container.");

            return _container.Resolve<T>();
        }

        public static IEnumerable<T> GetAll<T>() where T : class
        {
            if (_container == null)
                throw new InvalidOperationException("Could not resolve the current container.");

            return _container.ResolveAll<T>();
        }

        public static T Get<T>(IDictionary<string, object> constructorParameters) where T : class
        {
            if (_container == null)
                throw new InvalidOperationException("Could not resolve the current container.");

            var param = new NamedParameterOverloads(constructorParameters);

            return _container.Resolve<T>(param);
        }

        public static void BuildUp(object input)
        {
            _container.BuildUp(input, ResolveOptions.FailUnregisteredAndNameNotFound);
        }
    }
}
