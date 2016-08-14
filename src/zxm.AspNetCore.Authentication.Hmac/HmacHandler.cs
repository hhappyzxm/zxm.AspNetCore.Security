using System;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Primitives;
using zxm.AspNetCore.Authentication.Hmac.Signature;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public class HmacHandler : AuthenticationHandler<HmacOptions>
    {
        private readonly SignatureOptions _signatureOptions;
        private string _signatureInRequest;

        public HmacHandler()
        {
            _signatureOptions = new SignatureOptions();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!VerifyHttpMethod())
            {
                return AuthenticateResult.Fail("Invalid http method.");
            }

            if (!TryParseSignatureOptions())
            {
                return AuthenticateResult.Fail("Invalid signature formate.");
            }

            var signature = SignatureFactory.GenerateSignature(_signatureOptions);
            if (!_signatureInRequest.Equals(signature, StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.Fail("Invalid signature.");
            }

            return null;
        }

        private bool VerifyHttpMethod()
        {
            return Request.Method.Equals("Post", StringComparison.OrdinalIgnoreCase);
        }

        private bool TryParseSignatureOptions()
        {
            if (!Request.QueryString.HasValue)
            {
                return false;
            }

            _signatureOptions.ClientId = Request.Query[SignatureKeys.ClientId];
            if(string.IsNullOrEmpty(_signatureOptions.ClientId)) return false;

            _signatureOptions.ClientSecret = Options.TryGetClientSecret(_signatureOptions.ClientId);
            if (string.IsNullOrEmpty(_signatureOptions.ClientSecret)) return false;

            _signatureInRequest = Request.Query[SignatureKeys.Signature];
            if (string.IsNullOrEmpty(_signatureInRequest)) return false;

            //_signatureOptions.Timestamp = Request.Query[SignatureKeys.Timestamp];

            _signatureOptions.AccessToken = Request.Query[SignatureKeys.AccessToken];

            if (Request.Body.Length > 0)
            {
                Request.Body.Position = 0;
                using (var streamReader = new StreamReader(Request.Body))
                {
                    _signatureOptions.PostData = streamReader.ReadToEnd();
                }
            }

            return true;
        }

        protected override Task HandleSignOutAsync(SignOutContext context)
        {
            throw new NotSupportedException();
        }

        protected override Task HandleSignInAsync(SignInContext context)
        {
            throw new NotSupportedException();
        }
    }
}
