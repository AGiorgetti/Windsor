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

namespace Castle.Windsor.Extensions.DependencyInjection.Scope
{
	using System;

	using Microsoft.Extensions.DependencyInjection;

	internal class ServiceScope : IServiceScope
	{
		private readonly string scopeId;

		public ServiceScope(string scopeId, IServiceProvider serviceProvider)
		{
			if (string.IsNullOrWhiteSpace(scopeId))
			{
				throw new ArgumentException($"'{nameof(scopeId)}' cannot be null or whitespace.", nameof(scopeId));
			}

			this.scopeId = scopeId;
			ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
		}

		public IServiceProvider ServiceProvider { get; }

		public void Dispose()
		{
			ExtensionContainerScopeCache.Dispose(scopeId);
			(ServiceProvider as IDisposable).Dispose();
		}
	}
}