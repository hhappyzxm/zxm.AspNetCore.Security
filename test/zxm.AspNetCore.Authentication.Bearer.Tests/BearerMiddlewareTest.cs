using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using zxm.AspNetCore.Authentication.Hmac;
using zxm.AspNetCore.Authorization.Hmac;
using zxm.AspNetCore.Authentication.Hmac.Signature;

namespace zxm.AspNetCore.Authentication.Bearer.Tests
{
    public class BearerMiddlewareTest
    {
        [Fact]
        public async Task TestBearerMiddler()
        {
            var hostBuilder = new WebHostBuilder();
            hostBuilder.ConfigureServices(collection =>
            {
                collection.AddAuthorization(options =>
                {
                    options.AddPolicy("LoggedUser",
                         policy => policy.Requirements.Add(new LoggedUserRequirement()));
                });
                collection.AddMvc();
            });
            hostBuilder.Configure(app =>
            {
                var options = new HmacOptions()
                {
                    TryGetClientSecret = clientId => clientId == "123456"?"456789":string.Empty,
                    VerifyLoggedUserToken = token => token == "abc"
                };
                app.UseHmacAuthentication(options);
                app.UseMvc();
            });

            using (var testServer = new TestServer(hostBuilder))
            {
                var signatureOptions = new SignatureOptions
                {
                    ClientId = "123456",
                    ClientSecret = "456789",
                    Timestamp = 10
                };
                var signature = BuildSignature(signatureOptions);
                var req1 = testServer.CreateRequest("/api/test/test1"+ BuildSignatureQueryString(signatureOptions, signature));
                var res1 = await req1.PostAsync();
                Assert.Equal(200, (int)res1.StatusCode);

                var req2 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res2 = await req2.PostAsync();
                Assert.Equal(403, (int)res2.StatusCode);

                signatureOptions.LoggedUserToken = "abc";
                signature = BuildSignature(signatureOptions);
                var req3 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res3 = await req3.PostAsync();
                Assert.Equal(200, (int)res3.StatusCode);

                signatureOptions.ClientId = "123123";
                signature = BuildSignature(signatureOptions);
                var req4 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res4 = await req4.PostAsync();
                Assert.Equal(401, (int)res4.StatusCode);
            }
        }

        private string BuildSignature(SignatureOptions options)
        {
            return SignatureFactory.GenerateSignature(options);
        }

        private string BuildSignatureQueryString(SignatureOptions options, string signature)
        {
            var queryStr = $"?clientid={options.ClientId}&timestamp={options.Timestamp}";

            if (!string.IsNullOrEmpty(options.LoggedUserToken))
            {
                queryStr += $"&loggedusertoken={options.LoggedUserToken}";
            }

            queryStr += $"&signature={signature}";

            return queryStr;
        }
    }
}
