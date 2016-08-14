using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
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

            var identityUser = new GenericIdentity(_signatureOptions.ClientId);

            if (!string.IsNullOrEmpty(_signatureOptions.LoggedUserToken) && Options.VerifyLoggedUserToken != null)
            {
                if (Options.VerifyLoggedUserToken(_signatureOptions.LoggedUserToken))
                {
                    identityUser.AddClaim(new Claim(ClaimTypes.UserData, _signatureOptions.LoggedUserToken));
                }
            }

            var principal = new ClaimsPrincipal(identityUser);
            var ticket = new AuthenticationTicket(principal, new AuthenticationProperties(), Options.AuthenticationScheme);
            return AuthenticateResult.Success(ticket);
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

            int tmpTimestamp;
            if (int.TryParse(Request.Query[SignatureKeys.Timestamp], out tmpTimestamp))
            {
                _signatureOptions.Timestamp = tmpTimestamp;
            }
            else
            {
                return false;
            }

            _signatureOptions.LoggedUserToken = Request.Query[SignatureKeys.LoggedUserToken];

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

        protected override Task<bool> HandleForbiddenAsync(ChallengeContext context)
        {
            return base.HandleForbiddenAsync(context);
        }

        protected override Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            return base.HandleUnauthorizedAsync(context);
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

