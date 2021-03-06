﻿// <copyright file="Expand_extension_tests.cs" company="Stormpath, Inc.">
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
using System.Threading.Tasks;
using Shouldly;
using Stormpath.SDK.Account;
using Xunit;

namespace Stormpath.SDK.Tests.Linq
{
    public class Expand_extension_tests : Linq_test<IAccount>
    {
        [Fact]
        public async Task Expand_one_link()
        {
            await this.Queryable
                .Expand(x => x.GetDirectory())
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=directory");
        }

        [Fact]
        public async Task Expand_multiple_links()
        {
            await this.Queryable
                .Expand(x => x.GetDirectory())
                .Expand(x => x.GetTenant())
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=directory,tenant");
        }

        [Fact]
        public async Task Expand_collection_query_with_no_parameters()
        {
            await this.Queryable
                .Expand(x => x.GetGroups())
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=groups");
        }

        [Fact]
        public async Task Expand_collection_query_with_offset()
        {
            await this.Queryable
                .Expand(x => x.GetGroups(10, null))
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=groups(offset:10)");
        }

        [Fact]
        public async Task Expand_collection_query_with_limit()
        {
            await this.Queryable
                .Expand(x => x.GetGroups(null, 20))
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=groups(limit:20)");
        }

        [Fact]
        public async Task Expand_collection_query_with_both_parameters()
        {
            await this.Queryable
                .Expand(x => x.GetGroups(5, 15))
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=groups(offset:5,limit:15)");
        }

        [Fact]
        public async Task Expand_all_the_things()
        {
            await this.Queryable
                .Expand(x => x.GetTenant())
                .Expand(x => x.GetGroups(10, 20))
                .Expand(x => x.GetDirectory())
                .MoveNextAsync();

            this.ShouldBeCalledWithArguments("expand=tenant,groups(offset:10,limit:20),directory");
        }

        [Fact]
        public async Task Throws_for_negative_offset()
        {
            // TODO ArgumentOutOfRangeException after Shouldly Mono fix
            await Should.ThrowAsync<Exception>(async () =>
            {
                await this.Queryable
                    .Expand(x => x.GetGroups(-1, 0))
                    .MoveNextAsync();
            });
        }

        [Fact]
        public void Throws_for_negative_limit()
        {
            // TODO ArgumentOutOfRangeException after Shouldly Mono fix
            Should.Throw<Exception>(async () =>
            {
                await this.Queryable
                    .Expand(x => x.GetGroups(0, -1))
                    .MoveNextAsync();
            });
        }
    }
}