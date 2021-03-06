﻿// <copyright file="IAccountSync.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Account;
using Stormpath.SDK.Api;
using Stormpath.SDK.Directory;
using Stormpath.SDK.Group;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Impl.Tenant;
using Stormpath.SDK.Provider;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Impl.Account
{
    /// <summary>
    /// Represents the synchronous actions that correspond to the default asynchronous actions
    /// available on <see cref="IAccount">Account</see>.
    /// </summary>
    internal interface IAccountSync :
        IHasTenantSync,
        ISaveableWithOptionsSync<IAccount>,
        IDeletableSync,
        IExtendableSync
    {
        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.GetDirectoryAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns>The Directory.</returns>
        IDirectory GetDirectory();

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.AddGroupAsync(IGroup, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="group">The Group this account will be added to.</param>
        /// <returns>
        /// The new <see cref="IGroupMembership">Group Membership</see> resource created reflecting the account-to-group association.
        /// </returns>
        IGroupMembership AddGroup(IGroup group);

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.AddGroupAsync(string, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="hrefOrName">The <c>href</c> or name of the group to add.</param>
        /// <returns>
        /// The new <see cref="IGroupMembership">Group Membership</see> resource created reflecting the account-to-group association.
        /// </returns>
        IGroupMembership AddGroup(string hrefOrName);

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.RemoveGroupAsync(IGroup, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="group">The group object from which the account must be removed.</param>
        /// <returns><see langword="true"/> if the operation succeeded; <see langword="false"/> otherwise.</returns>
        bool RemoveGroup(IGroup group);

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.RemoveGroupAsync(string, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="hrefOrName">The <c>href</c> or name of the group object from which the account must be removed.</param>
        /// <returns><see langword="true"/> if the operation succeeded; <see langword="false"/> otherwise.</returns>
        bool RemoveGroup(string hrefOrName);

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.IsMemberOfGroupAsync(string, System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="hrefOrName">The <c>href</c> or name of the group to check.</param>
        /// <returns><see langword="true"/> if the account belongs to the specified group.</returns>
        bool IsMemberOfGroup(string hrefOrName);

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.GetProviderDataAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns>The provider data.</returns>
        IProviderData GetProviderData();

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.CreateApiKeyAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <returns>The new API Key.</returns>
        IApiKey CreateApiKey();

        /// <summary>
        /// Synchronous counterpart to <see cref="IAccount.CreateApiKeyAsync(System.Threading.CancellationToken)"/>.
        /// </summary>
        /// <param name="retrievalOptionsAction">The options to apply to the request.</param>
        /// <returns>The new API Key.</returns>
        IApiKey CreateApiKey(Action<IRetrievalOptions<IApiKey>> retrievalOptionsAction);
    }
}
