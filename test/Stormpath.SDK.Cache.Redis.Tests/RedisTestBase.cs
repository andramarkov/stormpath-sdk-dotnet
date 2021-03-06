﻿// <copyright file="RedisTestBase.cs" company="Stormpath, Inc.">
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

using Stormpath.SDK.Client;
using Stormpath.SDK.Logging;
using Stormpath.SDK.Serialization;
using Stormpath.SDK.Tests.Common.Fakes;

namespace Stormpath.SDK.Cache.Redis.Tests
{
    public abstract class RedisTestBase
    {
        private static readonly string BaseUrl = "https://api.stormpath.com/v1";
        protected readonly RedisTestFixture fixture;

        protected ResourceReturningHttpClient fakeHttpClient;
        protected IClient client;
        protected ILogger logger;

        public RedisTestBase(RedisTestFixture fixture)
        {
            this.fixture = fixture;

            fixture.FlushDatabase();
        }

        protected void CreateClient(ICacheProvider cacheProvider)
        {
            this.logger = new InMemoryLogger();
            this.fakeHttpClient = new ResourceReturningHttpClient(BaseUrl);

            this.client = Clients.Builder()
                .SetApiKeyId("foobar")
                .SetApiKeySecret("secret123!")
                .SetHttpClient(this.fakeHttpClient)
                .SetSerializer(Serializers.Create().JsonNetSerializer())
                .SetCacheProvider(cacheProvider)
                .SetLogger(this.logger)
                .Build();
        }
    }
}
