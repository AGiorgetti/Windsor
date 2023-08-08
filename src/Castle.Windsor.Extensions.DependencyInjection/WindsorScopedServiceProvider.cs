// Copyright 2004-2020 Castle Project - http://www.castleproject.org/
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


namespace Castle.Windsor.Extensions.DependencyInjection
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using Castle.MicroKernel;
	using Castle.Windsor;

	using Microsoft.Extensions.DependencyInjection;

	internal class RootWinsorServiceProvider : WindsorScopedServiceProvider
	{
		public RootWinsorServiceProvider(IWindsorContainer container) : base(container, "root")
		{
		}
	}

	internal class WindsorScopedServiceProvider : IServiceProvider, ISupportRequiredService, IDisposable
	{
		private readonly IWindsorContainer container;
		private readonly string scopeId;
		private bool disposing;

		public WindsorScopedServiceProvider(IWindsorContainer container, string scopeId)
		{
			this.container = container;
			this.scopeId = scopeId;
		}

		public object GetService(Type serviceType)
		{
			return ResolveInstanceOrNull(serviceType, true);
		}

		public object GetRequiredService(Type serviceType)
		{
			return ResolveInstanceOrNull(serviceType, false);
		}

		public void Dispose()
		{
			if (scopeId == "root")
			{
				if (!disposing)
				{
					disposing = true;
					// i'm not really sure we should dispose the container if it comes from outside
					container.Dispose();
				}
			}
		}

		private object ResolveInstanceOrNull(Type serviceType, bool isOptional)
		{
			var args = new Arguments
			{
				{ "scopeId", scopeId }
			};

			if (container.Kernel.HasComponent(serviceType))
			{
				return container.Resolve(serviceType, args);
			}

			if (serviceType.GetTypeInfo().IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
			{
				var allObjects = container.ResolveAll(serviceType.GenericTypeArguments[0], args);
				return allObjects;
			}

			if (isOptional)
			{
				return null;
			}

			return container.Resolve(serviceType, args);
		}
	}
}