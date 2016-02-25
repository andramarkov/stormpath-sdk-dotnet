﻿// <copyright file="ResourceAction.cs" company="Stormpath, Inc.">
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
using Stormpath.SDK.Shared;

namespace Stormpath.SDK.Impl.DataStore
{
    internal sealed class ResourceAction : StringEnumeration
    {
        public static ResourceAction Create = new ResourceAction("CREATE");
        public static ResourceAction Read = new ResourceAction("READ");
        public static ResourceAction Update = new ResourceAction("UPDATE");
        public static ResourceAction Delete = new ResourceAction("DELETE");

        private ResourceAction(string value)
            : base(value)
        {
        }

        public static ResourceAction Parse(string action)
        {
            switch (action.ToUpper())
            {
                case "CREATE": return Create;
                case "READ": return Read;
                case "UPDATE": return Update;
                case "DELETE": return Delete;
                default:
                    throw new Exception($"Could not parse HTTP method value '{action.ToUpper()}'");
            }
        }
    }
}