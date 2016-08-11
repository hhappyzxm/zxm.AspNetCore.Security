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
                collection.AddAuthentication();
                collection.AddMvc();
            });
            hostBuilder.Configure(app =>
            {
                app.UseBearerAuthentication("123456");
                app.UseMvc();
                //app.Run(async context =>
                //{
                //    await context.Response.WriteAsync("Hello world");
                //});
            });

            using (var testServer = new TestServer(hostBuilder))
            {
                var response = await testServer.CreateRequest("/api/test").GetAsync();
                Assert.Equal(401, (int)response.StatusCode);

                var req1 = testServer.CreateRequest("/api/test");
                req1.AddHeader("Authorization", "Bearer 66666");
                var res1 = await req1.GetAsync();
                Assert.Equal(401, (int)res1.StatusCode);

                var req2 = testServer.CreateRequest("/api/test");
                req2.AddHeader("Authorization", "Bearer 123456");
                var res2 = await req2.GetAsync();
                Assert.Equal(200, (int)res2.StatusCode);
            }
        }
    }
}
