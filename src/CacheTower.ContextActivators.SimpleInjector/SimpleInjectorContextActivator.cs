using System;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace CacheTower.ContextActivators.SimpleInjector
{
    public class SimpleInjectorContextActivator<TContext> : ICacheContextActivator
    {
        private readonly Container _container;

        public SimpleInjectorContextActivator(Container container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container)); ;
        }

        public ICacheContextScope BeginScope()
        {
            return new SimpleInjectorContextScope<TContext>(
                _container,
                AsyncScopedLifestyle.BeginScope(_container));
        }
    }
}