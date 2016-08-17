using System.Net.Http;
using zxm.AspNetCore.WebApi.Result.Abstractions;

namespace zxm.AspNetCore.Hmac.Client
{
    public class HmacResponseMessage
    {
        public HttpResponseMessage ResponseMessage { get; set; }

        public IWebApiResult Result { get; set; }
    }
}
