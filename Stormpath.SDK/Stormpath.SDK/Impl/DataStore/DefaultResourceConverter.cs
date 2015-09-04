﻿// <copyright file="DefaultResourceConverter.cs" company="Stormpath, Inc.">
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

using System.Collections.Generic;
using System.Linq;
using Stormpath.SDK.Impl.Resource;
using Stormpath.SDK.Shared;

namespace Stormpath.SDK.Impl.DataStore
{
    internal sealed class DefaultResourceConverter : IResourceConverter
    {
        Dictionary<string, object> IResourceConverter.ToMap(AbstractResource resource)
        {
            var propertyNames = resource.GetUpdatedPropertyNames();

            var resultMap = propertyNames.Select(name =>
            {
                var rawValue = resource.GetProperty(name);
                var value = SanitizeValue(resource, name, rawValue);
                return new { name, value };
            }).ToDictionary(
                map => map.name,
                map => map.value);

            return resultMap;
        }

        private static object SanitizeValue(AbstractResource resource, string propertyName, object rawValue)
        {
            // TODO Handle CustomData: no sanitization

            // TODO Handle ProviderData, Provider, etc as special cases

            // TODO Convert all this to the converter/filter pattern used for FieldConverters?

            // TODO for now we are just skipping link properties; no need to serialize
            var asLinkProperty = rawValue as LinkProperty;
            if (asLinkProperty != null)
            {
                return null;
            }

            bool isEnumeration = rawValue.GetType().IsSubclassOf(typeof(Enumeration));
            if (isEnumeration)
            {
                return rawValue.ToString().ToLower();
            }

            return rawValue;
        }
    }
}