﻿// <copyright file="DefaultApplicationAccountStoreMapping.cs" company="Stormpath, Inc.">
// Copyright (c) 2016 Stormpath, Inc.
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
using System.Threading;
using System.Threading.Tasks;
using Stormpath.SDK.AccountStore;
using Stormpath.SDK.Application;
using Stormpath.SDK.Impl.AccountStore;
using Stormpath.SDK.Impl.Resource;

namespace Stormpath.SDK.Impl.Application
{
    internal sealed class DefaultApplicationAccountStoreMapping :
        AbstractAccountStoreMapping<IApplicationAccountStoreMapping>,
        IAccountStoreMapping,
        IAccountStoreMappingSync,
        IApplicationAccountStoreMapping
    {
        private static readonly string ApplicationPropertyName = "application";

        public DefaultApplicationAccountStoreMapping(ResourceData data)
            : base(data)
        {
        }

        internal IEmbeddedProperty Application => this.GetLinkProperty(ApplicationPropertyName);

        public override IApplicationAccountStoreMapping SetApplication(IApplication application)
        {
            if (string.IsNullOrEmpty(application?.Href))
            {
                throw new ArgumentNullException(nameof(application.Href));
            }

            this.SetLinkProperty(ApplicationPropertyName, application.Href);

            return this;
        }

        public override Task<IApplication> GetApplicationAsync(CancellationToken cancellationToken)
            => this.GetInternalAsyncDataStore().GetResourceAsync<IApplication>(this.Application.Href, cancellationToken);

        public override IApplication GetApplication()
            => this.GetInternalSyncDataStore().GetResource<IApplication>(this.Application.Href);
    }
}