﻿// <copyright file="IFacebookProvider.cs" company="Stormpath, Inc.">
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

namespace Stormpath.SDK.Provider
{
    /// <summary>
    /// Facebook-specific <see cref="IProvider">Provider</see> Resource.
    /// </summary>
    public interface IFacebookProvider : IProvider
    {
        /// <summary>
        /// Gets the App ID of the Facebook application.
        /// </summary>
        /// <value>The App ID of the Facebook application.</value>
        string ClientId { get; }

        /// <summary>
        /// Gets the App Secret of the Facebook application.
        /// </summary>
        /// <value>The App Secret of the Facebook application.</value>
        string ClientSecret { get; }
    }
}
