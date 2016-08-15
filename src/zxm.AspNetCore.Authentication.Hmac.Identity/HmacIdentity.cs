using System.Security.Claims;
using System.Security.Principal;

namespace zxm.AspNetCore.Authentication.Hmac.Identity
{
    public class HmacIdentity : GenericIdentity
    {
        public HmacIdentity(string clientId) : base(clientId)
        {
            ClientId = clientId;
        }

        public string ClientId { get; set; }

        public string UserAccessToken { get; set; }
    }

    //public class HmacIdentity : IIdentity
    //{
    //    public HmacIdentity(string clientId)
    //    {
    //        ClientId = clientId;
    //    }

    //    public string AuthenticationType { get; set; } = "HMAC";

    //    public bool IsAuthenticated { get; } = true;

    //    public string Name => ClientId;

    //    public string ClientId { get; set; }

    //    public string UserAccessToken { get; set; }
    //}
}
