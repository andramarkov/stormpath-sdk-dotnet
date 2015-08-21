﻿// <copyright file="DefaultClientApiKeyBuilder_tests.cs" company="Stormpath, Inc.">
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

using System;
using NSubstitute;
using Shouldly;
using Stormpath.SDK.Api;
using Stormpath.SDK.Impl.Api;
using Stormpath.SDK.Impl.Utility;
using Xunit;

namespace Stormpath.SDK.Tests.Impl
{
    public class DefaultClientApiKeyBuilder_tests
    {
        public class With_missing_values
        {
            private IClientApiKeyBuilder builder;

            public With_missing_values()
            {
                this.builder = new DefaultClientApiKeyBuilder(
                    Substitute.For<IConfigurationManager>(),
                    Substitute.For<IEnvironment>(),
                    Substitute.For<IFile>());
            }

            [Fact]
            public void With_missing_id_throws_error()
            {
                Assert.Throws<ApplicationException>(() =>
                {
                    var clientApiKey = builder
                        .SetSecret("foo")
                        .Build();
                });
            }

            [Fact]
            public void With_missing_secret_throws_error()
            {
                Assert.Throws<ApplicationException>(() =>
                {
                    var clientApiKey = builder
                        .SetId("foo")
                        .Build();
                });
            }
        }

        public class With_default_properties_file
        {
            private readonly string defaultLocation =
                System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"), ".stormpath\\", "apiKey.properties");

            private readonly string fileContents =
                "apiKey.id = 144JVZINOF5EBNCMG9EXAMPLE\r\n" +
                "apiKey.secret = lWxOiKqKPNwJmSldbiSkEbkNjgh2uRSNAb+AEXAMPLE";

            private IClientApiKeyBuilder builder;
            private IFile file;

            public With_default_properties_file()
            {
                this.file = Substitute.For<IFile>();
                this.file.ReadAllText(defaultLocation).Returns(fileContents);

                this.builder = new DefaultClientApiKeyBuilder(Substitute.For<IConfigurationManager>(), Substitute.For<IEnvironment>(), file);
            }

            [Fact]
            public void Loads_values_from_default_file()
            {
                var clientApiKey = this.builder.Build();

                clientApiKey.GetId().ShouldBe("144JVZINOF5EBNCMG9EXAMPLE");
                clientApiKey.GetSecret().ShouldBe("lWxOiKqKPNwJmSldbiSkEbkNjgh2uRSNAb+AEXAMPLE");
            }

            [Fact]
            public void Is_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Observes_property_name_settings()
            {
                var modifiedFile = fileContents
                    .Replace("apiKey.id", "myProps.ID")
                    .Replace("apiKey.secret", "myProps.SECRET");
                this.file.ReadAllText(defaultLocation).Returns(modifiedFile);

                var clientApiKey = this.builder
                    .SetIdPropertyName("myProps.ID")
                    .SetSecretPropertyName("myProps.SECRET")
                    .Build();
                clientApiKey.GetId().ShouldBe("144JVZINOF5EBNCMG9EXAMPLE");
                clientApiKey.GetSecret().ShouldBe("lWxOiKqKPNwJmSldbiSkEbkNjgh2uRSNAb+AEXAMPLE");
            }
        }

        public class With_environment_variable_file
        {
            private readonly string envVariableName = "STORMPATH_API_KEY_FILE";
            private readonly string testLocation = "envfile.properties";
            private readonly string fileContents =
                "apiKey.id = envId\r\n" +
                "apiKey.secret = envSecret\r\n";

            private IClientApiKeyBuilder builder;
            private IEnvironment env;
            private IFile file;

            public With_environment_variable_file()
            {
                this.env = Substitute.For<IEnvironment>();
                this.env.GetEnvironmentVariable(envVariableName).Returns(testLocation);

                this.file = Substitute.For<IFile>();
                this.file.ReadAllText(testLocation).Returns(fileContents);

                this.builder = new DefaultClientApiKeyBuilder(Substitute.For<IConfigurationManager>(), env, file);
            }

            [Fact]
            public void Loads_values_from_file()
            {
                var clientApiKey = this.builder.Build();

                clientApiKey.GetId().ShouldBe("envId");
                clientApiKey.GetSecret().ShouldBe("envSecret");
            }

            [Fact]
            public void Is_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Observes_property_name_settings()
            {
                var modifiedFile = fileContents
                    .Replace("apiKey.id", "env.ID")
                    .Replace("apiKey.secret", "env.SECRET");
                this.file.ReadAllText(testLocation).Returns(modifiedFile);

                var clientApiKey = this.builder
                    .SetIdPropertyName("env.ID")
                    .SetSecretPropertyName("env.SECRET")
                    .Build();
                clientApiKey.GetId().ShouldBe("envId");
                clientApiKey.GetSecret().ShouldBe("envSecret");
            }
        }

        public class Building_with_environment_variable_values
        {
            private readonly string idVariableName = "STORMPATH_API_KEY_ID";
            private readonly string fakeApiKeyId = "idSetByEnv";
            private readonly string secretVariableName = "STORMPATH_API_KEY_SECRET";
            private readonly string fakeApiSecretId = "secretSetByEnv";

            private IClientApiKeyBuilder builder;
            private IEnvironment env;

            public Building_with_environment_variable_values()
            {
                this.env = Substitute.For<IEnvironment>();
                this.env.GetEnvironmentVariable(idVariableName).Returns(fakeApiKeyId);
                this.env.GetEnvironmentVariable(secretVariableName).Returns(fakeApiSecretId);

                this.builder = new DefaultClientApiKeyBuilder(Substitute.For<IConfigurationManager>(), env, Substitute.For<IFile>());
            }

            [Fact]
            public void Loads_values_from_environment_variables()
            {
                var clientApiKey = this.builder.Build();

                clientApiKey.GetId().ShouldBe(fakeApiKeyId);
                clientApiKey.GetSecret().ShouldBe(fakeApiSecretId);
            }

            [Fact]
            public void Is_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Observes_property_name_settings()
            {
                var newKeyName = "different_env_apikeyid";
                var newSecretName = "different_env_secret";

                this.env = Substitute.For<IEnvironment>();
                this.env.GetEnvironmentVariable(newKeyName).Returns(fakeApiKeyId);
                this.env.GetEnvironmentVariable(newSecretName).Returns(fakeApiSecretId);
                this.builder = new DefaultClientApiKeyBuilder(Substitute.For<IConfigurationManager>(), env, Substitute.For<IFile>());

                var clientApiKey = this.builder
                    .SetIdPropertyName(newKeyName)
                    .SetSecretPropertyName(newSecretName)
                    .Build();
                clientApiKey.GetId().ShouldBe(fakeApiKeyId);
                clientApiKey.GetSecret().ShouldBe(fakeApiSecretId);
            }
        }

        public class Building_with_file_location_from_AppConfig
        {
            private readonly string configVariableName = "STORMPATH_API_KEY_FILE";
            private readonly string testLocation = "appConfig.properties";
            private readonly string fileContents =
                "apiKey.id = fromAppConfig?\r\n" +
                "apiKey.secret = fooSecret!\r\n";

            private IClientApiKeyBuilder builder;
            private IConfigurationManager config;
            private IFile file;

            public Building_with_file_location_from_AppConfig()
            {
                this.config = Substitute.For<IConfigurationManager>();
                this.config.AppSettings.Returns(new System.Collections.Specialized.NameValueCollection()
                {
                    { configVariableName, testLocation }
                });

                this.file = Substitute.For<IFile>();
                this.file.ReadAllText(testLocation).Returns(fileContents);

                this.builder = new DefaultClientApiKeyBuilder(config, Substitute.For<IEnvironment>(), file);
            }

            [Fact]
            public void Loads_values_from_file()
            {
                var clientApiKey = this.builder.Build();

                clientApiKey.GetId().ShouldBe("fromAppConfig?");
                clientApiKey.GetSecret().ShouldBe("fooSecret!");
            }

            [Fact]
            public void Iis_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Observes_property_name_settings()
            {
                var modifiedFile = fileContents
                    .Replace("apiKey.id", "appConfigFile.ID")
                    .Replace("apiKey.secret", "appConfigFile.secret");
                this.file.ReadAllText(testLocation).Returns(modifiedFile);

                var clientApiKey = this.builder
                    .SetIdPropertyName("appConfigFile.ID")
                    .SetSecretPropertyName("appConfigFile.secret")
                    .Build();
                clientApiKey.GetId().ShouldBe("fromAppConfig?");
                clientApiKey.GetSecret().ShouldBe("fooSecret!");
            }
        }

        public class Building_with_values_from_AppConfig
        {
            private readonly string idVariableName = "STORMPATH_API_KEY_ID";
            private readonly string fakeApiKeyId = "idSetByAppConfig";
            private readonly string secretVariableName = "STORMPATH_API_KEY_SECRET";
            private readonly string fakeApiSecretId = "secretSetByAppConfig";

            private IClientApiKeyBuilder builder;
            private IConfigurationManager config;

            public Building_with_values_from_AppConfig()
            {
                this.config = Substitute.For<IConfigurationManager>();
                this.config.AppSettings.Returns(new System.Collections.Specialized.NameValueCollection()
                {
                    { idVariableName, fakeApiKeyId },
                    { secretVariableName, fakeApiSecretId }
                });

                this.builder = new DefaultClientApiKeyBuilder(config, Substitute.For<IEnvironment>(), Substitute.For<IFile>());
            }

            [Fact]
            public void Loads_values_from_AppConfig_values()
            {
                var clientApiKey = this.builder.Build();

                clientApiKey.GetId().ShouldBe(fakeApiKeyId);
                clientApiKey.GetSecret().ShouldBe(fakeApiSecretId);
            }

            [Fact]
            public void Is_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Observes_property_name_settings()
            {
                var newKeyIdName = "myApiKeyId";
                var newKeySecretName = "myApiKeySecret";

                this.config = Substitute.For<IConfigurationManager>();
                this.config.AppSettings.Returns(new System.Collections.Specialized.NameValueCollection()
                {
                    { newKeyIdName, fakeApiKeyId },
                    { newKeySecretName, fakeApiSecretId }
                });
                this.builder = new DefaultClientApiKeyBuilder(config, Substitute.For<IEnvironment>(), Substitute.For<IFile>());

                var clientApiKey = this.builder
                    .SetIdPropertyName(newKeyIdName)
                    .SetSecretPropertyName(newKeySecretName)
                    .Build();

                clientApiKey.GetId().ShouldBe(fakeApiKeyId);
                clientApiKey.GetSecret().ShouldBe(fakeApiSecretId);
            }
        }

        public class Building_with_stream
        {
            private readonly string streamContents =
                "apiKey.id = streams_r_neet\r\n" +
                "apiKey.secret = pls_dont_steal!\r\n";

            private IClientApiKeyBuilder builder;

            public Building_with_stream()
            {
                this.builder = new DefaultClientApiKeyBuilder(
                    Substitute.For<IConfigurationManager>(),
                    Substitute.For<IEnvironment>(),
                    Substitute.For<IFile>());
            }

            [Fact]
            public void Loads_values_from_stream()
            {
                using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(streamContents)))
                {
                    var clientApiKey = this.builder
                        .SetInputStream(stream)
                        .Build();

                    clientApiKey.GetId().ShouldBe("streams_r_neet");
                    clientApiKey.GetSecret().ShouldBe("pls_dont_steal!");
                }
            }

            public void Is_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Stream_values_observe_property_name_settings()
            {
                var modifiedContents = streamContents
                    .Replace("apiKey.id", "my.id")
                    .Replace("apiKey.secret", "my.secret");

                using (var stream = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(modifiedContents)))
                {
                    var clientApiKey = this.builder
                        .SetInputStream(stream)
                        .SetIdPropertyName("my.id")
                        .SetSecretPropertyName("my.secret")
                        .Build();

                    clientApiKey.GetId().ShouldBe("streams_r_neet");
                    clientApiKey.GetSecret().ShouldBe("pls_dont_steal!");
                }
            }
        }

        public class Building_with_properties_file
        {
            private readonly string testLocation = "test.properties";
            private readonly string fileContents =
                "apiKey.id = foobar\r\n" +
                "apiKey.secret = bazsecret!\r\n";

            private IClientApiKeyBuilder builder;
            private IFile file;

            public Building_with_properties_file()
            {
                this.file = Substitute.For<IFile>();
                this.file.ReadAllText(testLocation)
                    .Returns(fileContents);

                this.builder = new DefaultClientApiKeyBuilder(Substitute.For<IConfigurationManager>(), Substitute.For<IEnvironment>(), file);
            }

            [Fact]
            public void With_properties_file()
            {
                var clientApiKey = this.builder
                    .SetFileLocation(testLocation)
                    .Build();

                clientApiKey.GetId().ShouldBe("foobar");
                clientApiKey.GetSecret().ShouldBe("bazsecret!");
            }

            [Fact]
            public void Properties_file_is_lower_priority_than_explicit()
            {
                var clientApiKey = this.builder
                    .SetFileLocation(testLocation)
                    .SetId("different")
                    .SetSecret("also_different")
                    .Build();

                clientApiKey.GetId().ShouldBe("different");
                clientApiKey.GetSecret().ShouldBe("also_different");
            }

            [Fact]
            public void Properties_file_observes_property_name_settings()
            {
                var modifiedFile = fileContents
                    .Replace("apiKey.id", "myfile.Id")
                    .Replace("apiKey.secret", "myfile.Secret");
                this.file.ReadAllText(testLocation).Returns(modifiedFile);

                var clientApiKey = this.builder
                    .SetFileLocation(testLocation)
                    .SetIdPropertyName("myfile.Id")
                    .SetSecretPropertyName("myfile.Secret")
                    .Build();
                clientApiKey.GetId().ShouldBe("foobar");
                clientApiKey.GetSecret().ShouldBe("bazsecret!");
            }

            [Fact]
            public void File_location_interprets_tilde_as_home_directory_on_windows()
            {
                var homeDir = Environment.ExpandEnvironmentVariables("%homedrive%%homepath%");

                this.file = Substitute.For<IFile>();
                this.file.ReadAllText($"{homeDir}\\test.properties")
                    .Returns(fileContents);

                var env = Substitute.For<IEnvironment>();
                env.ExpandEnvironmentVariables(Arg.Any<string>())
                    .Returns(call => Environment.ExpandEnvironmentVariables(call.Arg<string>()));

                this.builder = new DefaultClientApiKeyBuilder(Substitute.For<IConfigurationManager>(), env, file);

                var clientApiKey = this.builder
                    .SetFileLocation("~\\test.properties")
                    .Build();

                clientApiKey.GetId().ShouldBe("foobar");
                clientApiKey.GetSecret().ShouldBe("bazsecret!");
            }
        }

        public class Building_with_explicit_values
        {
            private IClientApiKeyBuilder builder;

            public Building_with_explicit_values()
            {
                this.builder = new DefaultClientApiKeyBuilder(
                    Substitute.For<IConfigurationManager>(),
                    Substitute.For<IEnvironment>(),
                    Substitute.For<IFile>());
            }

            [Fact]
            public void Uses_expicit_values()
            {
                var clientApiKey = this.builder
                    .SetId("foo")
                    .SetSecret("bar")
                    .Build();

                clientApiKey.GetId().ShouldBe("foo");
                clientApiKey.GetSecret().ShouldBe("bar");
            }
        }
    }
}
