﻿// <copyright file="SyncRefreshGrantAuthenticatorExtensions.cs" company="Stormpath, Inc.">
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

using Stormpath.SDK.Impl.Oauth;
using Stormpath.SDK.Oauth;

namespace Stormpath.SDK.Sync
{
    /// <summary>
    /// Provides synchronous access to the methods available on <see cref="IRefreshGrantAuthenticator"/>.
    /// </summary>
    public static class SyncRefreshGrantAuthenticatorExtensions
    {
        /// <summary>
        /// Synchronously executes the OAuth 2.0 Authentication Request and returns the result.
        /// </summary>
        /// <param name="authenticator">The <see cref="IRefreshGrantAuthenticator"/>.</param>
        /// <param name="authenticationRequest">The Authentication Request this authenticator will attempt.</param>
        /// <returns>An Authentication Result representing the successful authentication.</returns>
        /// <exception cref="Error.ResourceException">The authentication failed.</exception>
        public static IOauthGrantAuthenticationResult Authenticate(this IRefreshGrantAuthenticator authenticator, IRefreshGrantRequest authenticationRequest)
            => (authenticator as IRefreshGrantAuthenticatorSync).Authenticate(authenticationRequest);
    }
}
