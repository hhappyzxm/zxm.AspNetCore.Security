using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace zxm.AspNetCore.Hmac.Sample.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ValuesController : Controller
    {
        [HttpPost]
        public Task Test1()
        {
            return Task.FromResult(0);
        }

        [HttpPost]
        [Authorize("User")]
        public Task<string> Test2([FromBody]Model model)
        {
            return Task.FromResult(model.Id);
        }

        [HttpPost]
        public Task<string> Test3([FromBody]string id)
        {
            return Task.FromResult(id);
        }
    }

    public class Model
    {
        public string Id { get; set; }
    }
}
