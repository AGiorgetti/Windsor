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
	/// <summary>
	/// There should be just one root scope per container.
	/// It is used to check if we can decommission the container during <see cref="WindsorScopedServiceProvider"/>
	/// </summary>
	internal class ExtensionContainerRootScope : ExtensionContainerScopeBase
	{
		internal static ExtensionContainerRootScope RootScope { get; private set; }

		private ExtensionContainerRootScope()
		{ }

		public static ExtensionContainerRootScope BeginRootScope()
		{
			var scope = new ExtensionContainerRootScope();
			ExtensionContainerScopeCache.Current = scope;
			RootScope = scope;
			return scope;
		}
	}
}
