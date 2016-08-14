using System.Text;

namespace zxm.AspNetCore.Authentication.Hmac.Signature
{
    public class SignatureOptions
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public int Timestamp { get; set; }

        public string AccessToken { get; set; }

        public string PostData { get; set; }

        public Encoding Encoding { get; set; }
    }
}
