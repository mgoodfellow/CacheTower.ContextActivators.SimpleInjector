using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CacheTower.SimpleInjectorContext
{
    public class SimpleInjectorCacheContextActivator<TContext> : ICacheContextActivator
    {
        private readonly Container _container;

        public SimpleInjectorCacheContextActivator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container)); ;
        }

        public ICacheContextScope BeginScope()
        {
            return new SimpleInjectorCacheContextScope<TContext>(
                _container,
                AsyncScopedLifestyle.BeginScope(_container));
        }
    }
}