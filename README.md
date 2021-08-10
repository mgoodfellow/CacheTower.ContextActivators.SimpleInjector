# CacheTower.SimpleInjectorContext

[![NuGet version (CacheTower.SimpleInjectorContext)](https://img.shields.io/nuget/v/CacheTower.SimpleInjectorContext.svg?style=flat-square)](https://www.nuget.org/packages/CacheTower.SimpleInjectorContext/)

## Usage

Define a context, or use a pre-registered context

```csharp
public class SessionContext 
{
    public IStatsClient StatsClient { get; }
    public ISession Session { get; }

    public SessionContext(IStatsClient statsClient, ISession session)
    {
        StatsClient = statsClient;
        Session = session;
    }
}
```

Make sure you register your context (use interfaces if preferred, concrete shown for speed)

```csharp
container.Register<SessionContext>(Lifestyle.Scoped);
```

Configure your cache stack:

```csharp
var myCacheStack = new CacheStack<SessionContext>(
    new SimpleInjectorCacheContextActivator<SessionContext>(container),
    new ICacheLayer[]
        {
            new MemoryCacheLayer(),
        };,
    new ICacheExtension[]
        {
            new AutoCleanupExtension(TimeSpan.FromSeconds(30)),
        });

container.RegisterInstance<ICacheStack>(myCacheStack);
container.RegisterInstance<ICacheStack<SessionContext>>(myCacheStack);
```

Note that both ICacheStack and ICacheStack<SessionContext> are registered above, this just means when using code that doesn't care about the context type, you can call ICacheStack without providing a TContext

Finally, consume your context as required:

```csharp
public class MyClass 
{
    private ICacheStack<SessionContext> _cacheStack;

    public MyClass(ICacheStack<SessionContext cacheStack) 
    {
        _cacheStack = cacheStack;
    }

    public async Task DoSomething() 
    {
        var value = _cacheStack.GetOrSetAsync<TData>(
            "key",
            async (oldVal, context) => 
            {
                return await context.Session.Query<TData>().Where(x => x.Id == 1).SingleOrDefaultAsync);
            },
            new CacheSettings(TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(15)));

        // Do something with value
    }
}
```