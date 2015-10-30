﻿// <copyright file="Tenant_tests.cs" company="Stormpath, Inc.">
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

using Shouldly;
using Stormpath.SDK.Sync;
using Xunit;

namespace Stormpath.SDK.Tests.Integration.Sync
{
    [CollectionDefinition("Live tenant tests")]
    public class Tenant_tests
    {
        [Theory]
        [MemberData(nameof(IntegrationTestClients.GetClients), MemberType = typeof(IntegrationTestClients))]
        public void Getting_current_tenant(TestClientProvider clientBuilder)
        {
            var client = clientBuilder.GetClient();
            var tenant = client.GetCurrentTenant();

            tenant.ShouldNotBe(null);
            tenant.Href.ShouldNotBe(null);
            tenant.Name.ShouldNotBe(null);
        }
    }
}