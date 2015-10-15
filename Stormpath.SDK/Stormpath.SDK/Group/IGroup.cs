﻿// <copyright file="IGroup.cs" company="Stormpath, Inc.">
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

using System.Threading;
using System.Threading.Tasks;
using Stormpath.SDK.Account;
using Stormpath.SDK.AccountStore;
using Stormpath.SDK.Directory;
using Stormpath.SDK.Linq;
using Stormpath.SDK.Resource;

namespace Stormpath.SDK.Group
{
    public interface IGroup : IResource, ISaveable<IGroup>, IDeletable, IAuditable, IExtendable, IAccountStore
    {
        /// <summary>
        /// Gets the group's name, guaranteed to be unique for all groups within a <see cref="IDirectory"/>.
        /// </summary>
        /// <value>This group's name, guaranteed to be unique for all groups within a <see cref="IDirectory"/>.</value>
        string Name { get; }

        /// <summary>
        /// Gets the group's description.
        /// </summary>
        /// <value>This group's description. This is an optional property and may be null or empty.</value>
        string Description { get; }

        /// <summary>
        /// Gets the group's status.
        /// </summary>
        /// <value>
        /// This directory's status.
        /// An <see cref="DirectoryStatus.Enabled"/> directory may be used by applications to login accounts found within the directory.
        /// A <see cref="DirectoryStatus.Disabled"/> directory prevents its accounts from being used to login to applications.
        /// </value>
        GroupStatus Status { get; }

        /// <summary>
        /// Sets the group's description.
        /// </summary>
        /// <param name="description">The group's description. This is an optional property and may be <c>null</c> or empty.</param>
        /// <returns>This instance for method chaining.</returns>
        IGroup SetDescription(string description);

        /// <summary>
        /// Sets the group's name.
        /// </summary>
        /// <param name="name">The group's name. Group names are required and must be unique within a <see cref="IDirectory"/>.</param>
        /// <returns>This instance for method chaining.</returns>
        IGroup SetName(string name);

        /// <summary>
        /// Sets the group's status.
        /// </summary>
        /// <param name="status">The group's status.
        /// If a group is mapped to an <see cref="Application.IApplication"/> as an Account Store (for login purposes),
        /// and the Group is disabled, the accounts within that Group cannot login to the application. Accounts in enabled
        /// Groups mapped to an Application may login to that application.
        /// </param>
        /// <returns>This instance for method chaining.</returns>
        IGroup SetStatus(GroupStatus status);

        /// <summary>
        /// Assigns the specified <see cref="IAccount"/> to this <see cref="IGroup"/>.
        /// </summary>
        /// <param name="account">The account to assignt to this group.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A Task whose result is the new <see cref="IGroupMembership"/> resource created
        /// reflecting the group-to-account association.
        /// </returns>
        Task<IGroupMembership> AddAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Assigns this <see cref="IGroup"/> to the specified <see cref="IAccount"/>
        /// represented by its (case-insensitive) <c>username</c>, <c>email</c>, or <c>href</c>
        /// </summary>
        /// <param name="hrefOrEmailOrUsername">The <c>href</c>, email, or username of the <see cref="IAccount"/> to associate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>
        /// A Task whose result is the new <see cref="IGroupMembership"/> resource created
        /// reflecting the group-to-account association.
        /// </returns>
        /// <exception cref="InvalidOperationException">The specified account could not be found.</exception>
        Task<IGroupMembership> AddAccountAsync(string hrefOrEmailOrUsername, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes this group's association with the specified <see cref="IAccount"/>.
        /// </summary>
        /// <param name="account">The <see cref="IAccount"/> object to disassociate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task whose result determines whether the operation succeeded.</returns>
        /// <exception cref="InvalidOperationException">The specified account does not belong to this group.</exception>
        Task<bool> RemoveAccountAsync(IAccount account, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Removes this Group's association with the specified <see cref="IAccount"/>
        /// represented by its (case-insensitive) <c>username</c>, <c>email</c>, or <c>href</c>
        /// </summary>
        /// <param name="hrefOrEmailOrUsername">The <see cref="IAccount"/> object to disassociate.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task whose result determines whether the operation succeeded.</returns>
        /// <exception cref="InvalidOperationException">The specified account does not belong to this group.</exception>
        Task<bool> RemoveAccountAsync(string hrefOrEmailOrUsername, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets the group's parent <see cref="IDirectory"/> (where the group is stored).
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task whose result is this group's parent <see cref="IDirectory"/>.</returns>
        Task<IDirectory> GetDirectoryAsync(CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Gets a queryable list of all of the memberships in which this group participates.
        /// </summary>
        /// <returns>An <see cref="IAsyncQueryable{IGroupMembership}"/> that may be used to asynchronously list or search memberships.</returns>
        IAsyncQueryable<IGroupMembership> GetAccountMemberships();

        /// <summary>
        /// Gets a queryable list of all accounts in this directory.
        /// </summary>
        /// <returns>An <see cref="IAsyncQueryable{IAccount}"/> that may be used to asynchronously list or search accounts.</returns>
        IAsyncQueryable<IAccount> GetAccounts();
    }
}
