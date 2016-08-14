using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace zxm.AspNetCore.Authentication.Bearer.Tests.Controllers
{
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
