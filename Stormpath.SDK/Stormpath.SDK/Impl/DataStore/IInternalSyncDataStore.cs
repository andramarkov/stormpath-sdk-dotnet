﻿// <copyright file="IInternalSyncDataStore.cs" company="Stormpath, Inc.">
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
using System.Collections.Generic;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Impl.DataStore
{
    internal interface IInternalSyncDataStore : IInternalDataStore, IDataStoreSync
    {
        CollectionResponsePage<T> GetCollection<T>(string href);

        T GetResource<T>(string href, Func<IDictionary<string, object>, Type> typeLookup)
            where T : class, IResource;

        T GetResource<T>(string href, IdentityMapOptions identityMapOptions)
            where T : class, IResource;

        T Create<T>(string parentHref, T resource)
            where T : class;

        T Create<T>(string parentHref, T resource, ICreationOptions options)
            where T : class;

        TReturned Create<T, TReturned>(string parentHref, T resource)
            where T : class
            where TReturned : class;

        TReturned Create<T, TReturned>(string parentHref, T resource, ICreationOptions options)
            where T : class
            where TReturned : class;

        TReturned Create<T, TReturned>(string parentHref, T resource, IdentityMapOptions identityMapOptions)
            where T : class
            where TReturned : class;

        T Save<T>(T resource)
            where T : class, IResource, ISaveable<T>;

        T Save<T>(T resource, string queryString)
            where T : class, IResource, ISaveable<T>;

        bool Delete<T>(T resource)
            where T : class, IResource, IDeletable;

        bool DeleteProperty(string parentHref, string propertyName);
    }
}