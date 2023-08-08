# Castle.Windsor.Extensions.DependencyInjection

This implementation partially rework how scopes are managed and
how registrations are replicated between IServiceCollection and IWindsorContainer.

- Singleton: there should be only one instance of the service in the whole application tied to the container that resolved it.
             I don't see the need of having a root scope for that nor a dedicated root scope accessor, you should always be
		 	 able to resolve singletons from any scope.
			 For Singleton we use the standard Castle Windsor lifestyle.
- Scoped: there should be a valid scope in order to resolve these kind of components.
          I don't like the idea of having them resolved tied to the root scope, it will 
	      make them singletons in practice.
- Transient: there should be a valid scope in order to resolve these kind of components.
             Having them resolved by the root scope can lead to memory leaks (they can be tracked
			 by Castle Windsor and they instances will live in memory untill the root container is 
		     disposed, if they implement IDisposable they can be tracked by the IServiceProvider too
		     leading tho the same memoty leak issue.
		
IServiceScopeFactory is supposed to be Singleton in Microsoft.Extensions.DependencyInjection and all the scopes
are created flat, there's no such thing as scope hierarchy. Destroying one scope should not have any influence on the others.

See: [IServiceScopeFactory is Documented Assumed to be a Singleton](https://github.com/dotnet/runtime/issues/67391)

Also checking the implementation in dotnet github repository confirm that scopes are always created by the root factory and
no hierarchy is involved.

So.. what if we implement this with child containers? each scope will have it's own child container
created by the root container and disposed when the scope is disposed (all the resolutions made by that container will be gone).
otherwise we can have a dedicated lifestyle for scoped components.

The biggest issue if when the Scope is not available (like in Threadpool.UnsafeQueueUserWorkItem).


Attention: according to tests if I dispose a service provider even its singleton goes away