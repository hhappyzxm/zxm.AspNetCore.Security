using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using zxm.AspNetCore.Authentication.Hmac;
using zxm.AspNetCore.Authorization.Hmac;
using zxm.AspNetCore.Authentication.Hmac.Signature;
using zxm.AspNetCore.WebApi.Result.Abstractions;

namespace zxm.AspNetCore.Authentication.Bearer.Tests
{
    public class HmacMiddlewareTest
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
                    VerifyUserAccessToken = token =>
                    {
                        if (token == "abc")
                        {
                            return VerifyUserAccessTokenState.Successed;
                        }
                        else if (token == "edf")
                        {
                            return VerifyUserAccessTokenState.Expired;
                        }
                        else
                        {
                            return VerifyUserAccessTokenState.Failed;
                        }
                    }
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
                var ret1 = await ConvertToWebApiResult(res1);
                Assert.Equal(200, (int)res1.StatusCode);
                Assert.Equal(true, ret1.Successed);
                
                var req2 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res2 = await req2.PostAsync();
                var ret2 = await ConvertToWebApiResult(res2);
                Assert.Equal(200, (int)res2.StatusCode);
                Assert.Equal(false, ret2.Successed);
                Assert.Equal(ErrorCode.MissingUserAccessToken, ret2.ErrorCode);

                signatureOptions.UserAccessToken = "abc";
                signature = BuildSignature(signatureOptions);
                var req3 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res3 = await req3.PostAsync();
                var ret3 = await ConvertToWebApiResult(res3);
                Assert.Equal(200, (int)res3.StatusCode);
                Assert.Equal(true, ret3.Successed);

                signatureOptions.ClientId = "123123";
                signature = BuildSignature(signatureOptions);
                var req4 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res4 = await req4.PostAsync();
                var ret4 = await ConvertToWebApiResult(res4);
                Assert.Equal(200, (int)res4.StatusCode);
                Assert.Equal(false, ret4.Successed);
                Assert.Equal(ErrorCode.InvalidClientId, ret4.ErrorCode);
                
                var req5 = testServer.CreateRequest("/api/test/test2");
                var res5 = await req5.PostAsync();
                var ret5 = await ConvertToWebApiResult(res5);
                Assert.Equal(200, (int)res5.StatusCode);
                Assert.Equal(false, ret5.Successed);
                Assert.Equal(ErrorCode.MissingClientId, ret5.ErrorCode);

                var req6 = testServer.CreateRequest("/api/test/test2?clientid=123456");
                var res6 = await req6.PostAsync();
                var ret6 = await ConvertToWebApiResult(res6);
                Assert.Equal(200, (int)res6.StatusCode);
                Assert.Equal(false, ret6.Successed);
                Assert.Equal(ErrorCode.MissingSignature, ret6.ErrorCode);

                var req7 = testServer.CreateRequest("/api/test/test2?clientid=123456&signature=asfasdfasd");
                var res7 = await req7.PostAsync();
                var ret7 = await ConvertToWebApiResult(res7);
                Assert.Equal(200, (int)res7.StatusCode);
                Assert.Equal(false, ret7.Successed);
                Assert.Equal(ErrorCode.MissingTimestamp, ret7.ErrorCode);

                signatureOptions.ClientId = "123456";
                signatureOptions.UserAccessToken = "edf";
                signature = BuildSignature(signatureOptions);
                var req8 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res8 = await req8.PostAsync();
                var ret8 = await ConvertToWebApiResult(res8);
                Assert.Equal(200, (int)res8.StatusCode);
                Assert.Equal(false, ret8.Successed);
                Assert.Equal(ErrorCode.UserAccessTokenExpired, ret8.ErrorCode);

                signatureOptions.UserAccessToken = "edf111";
                signature = BuildSignature(signatureOptions);
                var req9 = testServer.CreateRequest("/api/test/test2" + BuildSignatureQueryString(signatureOptions, signature));
                var res9 = await req9.PostAsync();
                var ret9 = await ConvertToWebApiResult(res9);
                Assert.Equal(200, (int)res9.StatusCode);
                Assert.Equal(false, ret9.Successed);
                Assert.Equal(ErrorCode.InvalidUserAccessToken, ret9.ErrorCode);
            }
        }

        private string BuildSignature(SignatureOptions options)
        {
            return SignatureFactory.GenerateSignature(options);
        }

        private string BuildSignatureQueryString(SignatureOptions options, string signature)
        {
            var queryStr = $"?clientid={options.ClientId}&timestamp={options.Timestamp}";

            if (!string.IsNullOrEmpty(options.UserAccessToken))
            {
                queryStr += $"&loggedusertoken={options.UserAccessToken}";
            }

            queryStr += $"&signature={signature}";

            return queryStr;
        }

        private async Task<WebApiResult> ConvertToWebApiResult(HttpResponseMessage res)
        {
            var content = await res.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<WebApiResult>(content);
        }
    }
}
