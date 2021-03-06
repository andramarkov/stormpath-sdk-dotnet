﻿// <copyright file="IClientApiKeyBuilder.cs" company="Stormpath, Inc.">
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

namespace Stormpath.SDK.Api
{
    /// <summary>
    /// A Builder design pattern used to construct <see cref="IClientApiKey">Client API Key</see> instances.
    /// </summary>
    /// <example>
    /// Read an API Key at the specified path:
    /// <code>
    ///     IClientApiKey apiKey = ClientApiKeys.Builder()
    ///         .SetFileLocation("path\\to\\apiKey.properties")
    ///         .Build();
    /// </code>
    /// Then, create a client:
    /// <code>
    /// IClient client = Clients.Builder()
    ///     .SetApiKey(apiKey)
    ///     .Build();
    /// </code>
    /// </example>
    [Obsolete("Use IClientBuilder")]
    public interface IClientApiKeyBuilder
    {
        /// <summary>
        /// Allows specifying the client's API Key ID value directly instead of reading it from a file or stream.
        /// </summary>
        /// <remarks>
        /// <b>Usage Warning:</b> It is almost always advisable to NOT use this method and instead use
        /// methods that accept a file or stream: these other methods would ideally acquire the API Key
        /// from a secure and private apiKey.properties file that is readable only by the process that uses the Stormpath SDK.
        /// </remarks>
        /// <param name="id">The API Key ID to use when communicating with Stormpath.</param>
        /// <returns>This instance for method chaining.</returns>
        [Obsolete("Use IClientBuilder.SetApiKeyId")]
        IClientApiKeyBuilder SetId(string id);

        /// <summary>
        /// Allows specifying the client's API Key Secret value directly instead of reading it from a file or stream.
        /// </summary>
        /// <remarks>See the security precautions outlined at <see cref="SetId(string)"/>.</remarks>
        /// <param name="secret">The API Key Secret to use when communicating with Stormpath.</param>
        /// <returns>This instance for method chaining.</returns>
        [Obsolete("Use IClientBuilder.SetApiKeySecret")]
        IClientApiKeyBuilder SetSecret(string secret);

        /// <summary>
        /// Creates a <see cref="IClientApiKey">Client API Key</see> instance from the specified input stream instead of reading from a file.
        /// See <see cref="SetFileLocation(string)"/> for the expected input format.
        /// </summary>
        /// <param name="stream">The input stream to consume.</param>
        /// <returns>This instance for method chaining.</returns>
        [Obsolete("This method will be removed in 1.0.")]
        IClientApiKeyBuilder SetInputStream(System.IO.Stream stream);

        /// <summary>
        /// Sets the location of the .properties file to load containing the API Key ID and Secret used by the Client to communicate with the Stormpath REST API.
        /// </summary>
        /// <remarks>
        /// <para>When the file is loaded, the following name/value pairs are expected to be present by default: apiKey.id, apiKey.secret</para>
        /// <para>If you want to control the property names used in the file, you may configure them via <see cref="SetIdPropertyName(string)"/> and <see cref="SetSecretPropertyName(string)"/>.</para>
        /// <para>Note: the tilde (~) character is expanded to %HOMEDRIVE%%HOMEPATH% on Windows, to match the behavior on *nix platforms.</para>
        /// </remarks>
        /// <param name="path">The relative or absolute path to the .properties file.</param>
        /// <returns>This instance for method chaining.</returns>
        [Obsolete("Use IClientBuilder.SetApiKeyFilePath")]
        IClientApiKeyBuilder SetFileLocation(string path);

        /// <summary>
        /// Sets the name used to represent the API Key ID when parsing a .properties file or stream.
        /// </summary>
        /// <param name="idPropertyName">The API Key ID name.</param>
        /// <returns>This instance for method chaining.</returns>
        [Obsolete("This method will be removed in 1.0.")]
        IClientApiKeyBuilder SetIdPropertyName(string idPropertyName);

        /// <summary>
        /// Sets the name used to represent the API Key Secret when parsing a .properties file or stream.
        /// </summary>
        /// <param name="secretPropertyName">The API Key Secret name</param>
        /// <returns>This instance for method chaining.</returns>
        [Obsolete("This method will be removed in 1.0.")]
        IClientApiKeyBuilder SetSecretPropertyName(string secretPropertyName);

        /// <summary>
        /// Constructs a new <see cref="IClientApiKey">Client API Key</see> instance based on the builder's current configuration state.
        /// </summary>
        /// <returns>A new <see cref="IClientApiKey">Client API Key</see> instance.</returns>
        /// <exception cref="System.Exception">No valid API Key ID and Secret can be found.</exception>
        [Obsolete("This method will be removed in 1.0.")]
        IClientApiKey Build();
    }
}
