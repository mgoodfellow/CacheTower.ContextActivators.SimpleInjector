using System;
using SimpleInjector;

namespace CacheTower.SimpleInjectorContext
{
    public class SimpleInjectorCacheContextScope<TContext> : ICacheContextScope
    {
        private readonly Container _container;
        private readonly Scope _scope;

        public SimpleInjectorCacheContextScope(Container container, Scope scope)
        {
            _container = container;
            _scope = scope;
        }

        public object Resolve(Type type)
        {
            return _container.GetInstance(type);
        }

        public void Dispose()
        {
            _scope?.Dispose();
        }
    }
}
