using System.Text;

namespace zxm.AspNetCore.Authentication.Hmac.Signature
{
    public class SignatureOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Timestamp { get; set; }

        public string UserAccessToken { get; set; }

        public string PostData { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;
    }
}
