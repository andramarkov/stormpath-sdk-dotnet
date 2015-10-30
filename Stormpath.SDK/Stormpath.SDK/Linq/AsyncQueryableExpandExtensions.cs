﻿// <copyright file="AsyncQueryableExpandExtensions.cs" company="Stormpath, Inc.">
// Copyright (c) 2015 Stormpath, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.SDK.Impl.Linq;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK
{
    public static class AsyncQueryableExpandExtensions
    {
        /// <summary>
        /// Retrives additional data in this request from a linked resource. This has no effect if caching is disabled on the <see cref="Client.IClient"/> object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="selector">A function to select a resource-returning method to expand.</param>
        /// <returns>An <see cref="IAsyncQueryable{T}"/> whose elements will include additional data selected by <paramref name="selector"/>.</returns>
        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Func<CancellationToken, Task>>> selector)
            where TSource : IResource
        {
            return source.Provider.CreateQuery(
                LinqHelper.MethodCall(
                    LinqHelper.GetMethodInfo(Sync.ExpandExtensions.Expand, (IQueryable<TSource>)null, selector),
                    source.Expression,
                    Expression.Quote(selector)));
        }

        /// <summary>
        /// Retrives additional data in this request from a linked resource. This has no effect if caching is disabled on the <see cref="Client.IClient"/> object.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="selector">A function to select a collection-returning method to expand.</param>
        /// <param name="offset">Set the paging offset of the expanded collection, or <c>null</c> to use the default offset.</param>
        /// <param name="limit">Set the paging limit of the expanded collection, or <c>null</c> to use the default limit.</param>
        /// <returns>An <see cref="IAsyncQueryable{T}"/> whose elements will include additional data selected by <paramref name="selector"/>.</returns>
        public static IAsyncQueryable<TSource> Expand<TSource>(this IAsyncQueryable<TSource> source, Expression<Func<TSource, Func<IAsyncQueryable>>> selector, int? offset = null, int? limit = null)
            where TSource : IResource
        {
            if (offset != null && offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (limit != null && limit < 0)
                throw new ArgumentOutOfRangeException(nameof(limit));

            return source.Provider.CreateQuery(
                LinqHelper.MethodCall(
                    LinqHelper.GetMethodInfo(Sync.ExpandExtensions.Expand, (IQueryable<TSource>)null, selector, offset, limit),
                    source.Expression,
                    Expression.Quote(selector),
                    Expression.Constant(offset, typeof(int?)),
                    Expression.Constant(limit, typeof(int?))));
        }
    }
}