﻿// <copyright file="DefaultClient.cs" company="Stormpath, Inc.">
//      Copyright (c) 2015 Stormpath, Inc.
// </copyright>
// <remarks>
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
// </remarks>

using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Stormpath.SDK.Account;
using Stormpath.SDK.Api;
using Stormpath.SDK.Application;
using Stormpath.SDK.Cache;
using Stormpath.SDK.Client;
using Stormpath.SDK.DataStore;
using Stormpath.SDK.Directory;
using Stormpath.SDK.Group;
using Stormpath.SDK.Http;
using Stormpath.SDK.Impl.DataStore;
using Stormpath.SDK.Impl.Extensions;
using Stormpath.SDK.Impl.Http;
using Stormpath.SDK.Impl.Tenant;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Serialization;
using Stormpath.SDK.Shared;
using Stormpath.SDK.Sync;
using Stormpath.SDK.Tenant;

namespace Stormpath.SDK.Impl.Client
{
    internal sealed class DefaultClient : IClient, IClientSync
    {
        private readonly IClientApiKey apiKey;
        private readonly string baseUrl;
        private readonly AuthenticationScheme authenticationScheme;
        private readonly int connectionTimeout;
        private readonly IWebProxy proxy;
        private readonly IJsonSerializer serializer;
        private readonly IHttpClient httpClient;
        private readonly ILogger logger;

        private readonly IInternalDataStore dataStore;
        private readonly IInternalAsyncDataStore dataStoreAsync;
        private readonly IInternalSyncDataStore dataStoreSync;

        private bool alreadyDisposed = false;

        private ITenant tenant;

        public DefaultClient(
            IClientApiKey apiKey,
            string baseUrl,
            AuthenticationScheme authenticationScheme,
            int connectionTimeout,
            IWebProxy proxy,
            IHttpClient httpClient,
            IJsonSerializer serializer,
            ICacheProvider cacheProvider,
            ILogger logger,
            TimeSpan identityMapExpiration)
        {
            if (apiKey == null || !apiKey.IsValid())
                throw new ArgumentException("API Key is not valid.");
            if (string.IsNullOrEmpty(baseUrl))
                throw new ArgumentNullException("Base URL cannot be empty.");
            if (connectionTimeout < 0)
                throw new ArgumentException("Timeout cannot be negative.");

            this.logger = logger;
            this.apiKey = apiKey;
            this.baseUrl = baseUrl;
            this.connectionTimeout = connectionTimeout;
            this.proxy = proxy;
            this.authenticationScheme = authenticationScheme;
            this.serializer = serializer;
            this.httpClient = httpClient;

            var requestExecutor = new DefaultRequestExecutor(httpClient, apiKey, authenticationScheme, this.logger);

            this.dataStore = new DefaultDataStore(requestExecutor, baseUrl, this.serializer, this.logger, cacheProvider, identityMapExpiration);
            this.dataStoreAsync = this.dataStore as IInternalAsyncDataStore;
            this.dataStoreSync = this.dataStore as IInternalSyncDataStore;
        }

        private IClient AsInterface => this;

        private IClientSync AsSyncInterface => this;

        private string CurrentTenantHref => this.tenant?.Href.Nullable() ?? "tenants/current";

        internal IClientApiKey ApiKey => this.apiKey;

        internal string BaseUrl => this.dataStoreAsync.BaseUrl;

        internal AuthenticationScheme AuthenticationScheme => this.authenticationScheme;

        internal int ConnectionTimeout => this.connectionTimeout;

        internal IWebProxy Proxy => this.proxy;

        internal IJsonSerializer Serializer => this.serializer;

        internal IHttpClient HttpClient => this.httpClient;

        internal IInternalDataStore DataStore => this.dataStore;

        private async Task EnsureTenantAsync(CancellationToken cancellationToken)
        {
            if (this.tenant == null)
                await this.AsInterface.GetCurrentTenantAsync(cancellationToken).ConfigureAwait(false);
        }

        private void EnsureTenant()
        {
            if (this.tenant == null)
                this.GetCurrentTenant();
        }

        T IDataStore.Instantiate<T>() => this.dataStore.Instantiate<T>();

        async Task<ITenant> IClient.GetCurrentTenantAsync(CancellationToken cancellationToken)
        {
            this.tenant = await this.dataStoreAsync
                .GetResourceAsync<ITenant>(this.CurrentTenantHref, new IdentityMapOptions() { StoreWithInfiniteExpiration = true }, cancellationToken)
                .ConfigureAwait(false);

            return this.tenant;
        }

        ITenant IClientSync.GetCurrentTenant()
        {
            this.tenant = this.dataStoreSync.GetResource<ITenant>(this.CurrentTenantHref, new IdentityMapOptions() { StoreWithInfiniteExpiration = true });

            return this.tenant;
        }

        Task<T> IDataStore.GetResourceAsync<T>(string href, CancellationToken cancellationToken)
            => this.dataStoreAsync.GetResourceAsync<T>(href, cancellationToken);

        T IDataStoreSync.GetResource<T>(string href)
            => this.dataStoreSync.GetResource<T>(href);

        async Task<IApplication> ITenantActions.CreateApplicationAsync(IApplication application, Action<ApplicationCreationOptionsBuilder> creationOptionsAction, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateApplicationAsync(application, creationOptionsAction, cancellationToken).ConfigureAwait(false);
        }

        IApplication ITenantActionsSync.CreateApplication(IApplication application, Action<ApplicationCreationOptionsBuilder> creationOptionsAction)
        {
            this.EnsureTenant();

            return this.tenant.CreateApplication(application, creationOptionsAction);
        }

        async Task<IApplication> ITenantActions.CreateApplicationAsync(IApplication application, IApplicationCreationOptions creationOptions, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateApplicationAsync(application, creationOptions, cancellationToken).ConfigureAwait(false);
        }

        IApplication ITenantActionsSync.CreateApplication(IApplication application, IApplicationCreationOptions creationOptions)
        {
            this.EnsureTenant();

            return this.tenant.CreateApplication(application, creationOptions);
        }

        async Task<IApplication> ITenantActions.CreateApplicationAsync(IApplication application, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateApplicationAsync(application, cancellationToken).ConfigureAwait(false);
        }

        IApplication ITenantActionsSync.CreateApplication(IApplication application)
        {
            this.EnsureTenant();

            return this.tenant.CreateApplication(application);
        }

        async Task<IApplication> ITenantActions.CreateApplicationAsync(string name, bool createDirectory, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateApplicationAsync(name, createDirectory, cancellationToken).ConfigureAwait(false);
        }

        IApplication ITenantActionsSync.CreateApplication(string name, bool createDirectory)
        {
            this.EnsureTenant();

            return this.tenant.CreateApplication(name, createDirectory);
        }

        async Task<IDirectory> ITenantActions.CreateDirectoryAsync(IDirectory directory, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateDirectoryAsync(directory, cancellationToken).ConfigureAwait(false);
        }

        IDirectory ITenantActionsSync.CreateDirectory(IDirectory directory)
        {
            this.EnsureTenant();

            return this.tenant.CreateDirectory(directory);
        }

        async Task<IDirectory> ITenantActions.CreateDirectoryAsync(IDirectory directory, Action<DirectoryCreationOptionsBuilder> creationOptionsAction, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateDirectoryAsync(directory, creationOptionsAction, cancellationToken).ConfigureAwait(false);
        }

        IDirectory ITenantActionsSync.CreateDirectory(IDirectory directory, Action<DirectoryCreationOptionsBuilder> creationOptionsAction)
        {
            this.EnsureTenant();

            return this.tenant.CreateDirectory(directory, creationOptionsAction);
        }

        async Task<IDirectory> ITenantActions.CreateDirectoryAsync(IDirectory directory, IDirectoryCreationOptions creationOptions, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateDirectoryAsync(directory, creationOptions, cancellationToken).ConfigureAwait(false);
        }

        IDirectory ITenantActionsSync.CreateDirectory(IDirectory directory, IDirectoryCreationOptions creationOptions)
        {
            this.EnsureTenant();

            return this.tenant.CreateDirectory(directory, creationOptions);
        }

        async Task<IDirectory> ITenantActions.CreateDirectoryAsync(string name, string description, DirectoryStatus status, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.CreateDirectoryAsync(name, description, status, cancellationToken).ConfigureAwait(false);
        }

        IDirectory ITenantActionsSync.CreateDirectory(string name, string description, DirectoryStatus status)
        {
            this.EnsureTenant();

            return this.tenant.CreateDirectory(name, description, status);
        }

        async Task<IAccount> ITenantActions.VerifyAccountEmailAsync(string token, CancellationToken cancellationToken)
        {
            await this.EnsureTenantAsync(cancellationToken).ConfigureAwait(false);

            return await this.tenant.VerifyAccountEmailAsync(token, cancellationToken).ConfigureAwait(false);
        }

        IAccount ITenantActionsSync.VerifyAccountEmail(string token)
        {
            this.EnsureTenant();

            return this.tenant.VerifyAccountEmail(token);
        }

        IAsyncQueryable<IApplication> ITenantActions.GetApplications()
        {
            this.EnsureTenant();

            return this.tenant.GetApplications();
        }

        IAsyncQueryable<IDirectory> ITenantActions.GetDirectories()
        {
            this.EnsureTenant();

            return this.tenant.GetDirectories();
        }

        IAsyncQueryable<IAccount> ITenantActions.GetAccounts()
        {
            this.EnsureTenant();

            return this.tenant.GetAccounts();
        }

        IAsyncQueryable<IGroup> ITenantActions.GetGroups()
        {
            this.EnsureTenant();

            return this.tenant.GetGroups();
        }

        private void Dispose(bool disposing)
        {
            if (!this.alreadyDisposed)
            {
                if (disposing)
                {
                    this.dataStore.Dispose();
                }

                this.alreadyDisposed = true;
            }
        }

        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            this.Dispose(true);
        }
    }
}
