﻿// <copyright file="SyncOauthPolicyExtensions.cs" company="Stormpath, Inc.">
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

using Stormpath.SDK.Application;
using Stormpath.SDK.Impl.Oauth;
using Stormpath.SDK.Oauth;

namespace Stormpath.SDK.Sync
{
    /// <summary>
    /// Provides synchronous access to the methods available on <see cref="IOauthPolicy"/>.
    /// </summary>
    public static class SyncOauthPolicyExtensions
    {
        /// <summary>
        /// Synchronously returns the <see cref="IApplication">Application</see> associated with this <see cref="IOauthPolicy">OauthPolicy</see>.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>The <see cref="IApplication">Application</see> associated with this <see cref="IOauthPolicy">OauthPolicy</see>.</returns>
        public static IApplication GetApplication(this IOauthPolicy policy)
            => (policy as IOauthPolicySync).GetApplication();
    }
}
