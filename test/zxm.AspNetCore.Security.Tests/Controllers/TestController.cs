using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using zxm.AspNetCore.WebApi.Result.Abstractions;
using zxm.AspNetCore.WebApi.ResultExtenstion;

namespace zxm.AspNetCore.Security.Tests.Controllers
{
    [ResultActionFilterAttributer]
    [Route("api/[controller]/[action]")]
    public class TestController : Controller
    {
        [HttpPost]
        [Authorize]
        public async Task<string> Test1()
        {
            return await Task.FromResult("Test1 is ok");
        }

        [HttpPost]
        [Authorize(Policy = "LoggedUser")]
        public async Task<string> Test2()
        {
            return await Task.FromResult("Test2 is ok");
        }
    }
}
