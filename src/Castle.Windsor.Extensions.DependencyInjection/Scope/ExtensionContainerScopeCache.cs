﻿// Copyright 2004-2020 Castle Project - http://www.castleproject.org/
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
	using Castle.MicroKernel.Lifestyle.Scoped;
	using System;
	using System.Collections.Concurrent;

	internal static class ExtensionContainerScopeCache
	{
		private static readonly ConcurrentDictionary<string, ILifetimeScope> collection = new ConcurrentDictionary<string, ILifetimeScope>();

		internal static ILifetimeScope GetOrAdd(string scopeId, Func<string, ILifetimeScope> valueFactory)
		{
			return collection.GetOrAdd(scopeId, valueFactory);
		}

		internal static void Dispose(string scopeId)
		{
			if (collection.TryGetValue(scopeId, out var scope))
			{
				scope.Dispose();
				collection.TryRemove(scopeId, out _);
			}
		}

		internal static void Dispose()
		{
			foreach (var scope in collection)
			{
				scope.Value.Dispose();
			}
			collection.Clear();
		}
	}
}