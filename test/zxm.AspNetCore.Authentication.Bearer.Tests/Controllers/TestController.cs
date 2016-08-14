using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace zxm.AspNetCore.Authentication.Bearer.Tests.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<string> Get()
        {
            return await Task.FromResult("Get is ok");
        }
    }
}
