using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using zxm.AspNetCore.Authentication.Hmac.Signature;
using zxm.AspNetCore.WebApi.Result.Abstractions;

namespace zxm.AspNetCore.Authentication.Hmac
{
    public class HmacHandler : AuthenticationHandler<HmacOptions>
    {
        private readonly SignatureOptions _signatureOptions;
        private string _signatureInRequest;
        private ErrorCode _errorCode;

        public HmacHandler()
        {
            _signatureOptions = new SignatureOptions();
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!VerifyHttpMethod())
            {
                _errorCode = ErrorCode.InvalidHttpMethod;
                return AuthenticateResult.Fail("Invalid http method.");
            }

            if (!TryParseSignatureOptions())
            {
                return AuthenticateResult.Fail("Invalid signature formate.");
            }

            var signature = SignatureFactory.GenerateSignature(_signatureOptions);
            if (!_signatureInRequest.Equals(signature, StringComparison.OrdinalIgnoreCase))
            {
                _errorCode = ErrorCode.InvalidSignature;
                return AuthenticateResult.Fail("Invalid signature.");
            }

            var identityUser = new GenericIdentity(_signatureOptions.ClientId);

            if (!string.IsNullOrEmpty(_signatureOptions.UserAccessToken) && Options.VerifyUserAccessToken != null)
            {
                var result = Options.VerifyUserAccessToken(_signatureOptions.UserAccessToken);
                if (result == VerifyUserAccessTokenState.Successed)
                {
                    identityUser.AddClaim(new Claim(ClaimTypes.UserData, _signatureOptions.UserAccessToken));
                }
                else if (result == VerifyUserAccessTokenState.Failed)
                {
                    _errorCode = ErrorCode.InvalidUserAccessToken;
                }
                else if (result == VerifyUserAccessTokenState.Expired)
                {
                    _errorCode = ErrorCode.UserAccessTokenExpired;
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
            _signatureOptions.ClientId = Request.Query[SignatureKeys.ClientId];
            if (string.IsNullOrEmpty(_signatureOptions.ClientId))
            {
                _errorCode = ErrorCode.MissingClientId;
                return false;
            }

            _signatureOptions.ClientSecret = Options.TryGetClientSecret(_signatureOptions.ClientId);
            if (string.IsNullOrEmpty(_signatureOptions.ClientSecret))
            {
                _errorCode = ErrorCode.InvalidClientId;
                return false;
            }

            _signatureInRequest = Request.Query[SignatureKeys.Signature];
            if (string.IsNullOrEmpty(_signatureInRequest))
            {
                _errorCode = ErrorCode.MissingSignature;
                return false;
            }
            
            int tmpTimestamp;
            if (int.TryParse(Request.Query[SignatureKeys.Timestamp], out tmpTimestamp))
            {
                _signatureOptions.Timestamp = tmpTimestamp;
            }
            else
            {
                _errorCode = ErrorCode.MissingTimestamp;
                return false;
            }

            _signatureOptions.UserAccessToken = Request.Query[SignatureKeys.LoggedUserToken];

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

        protected override async Task<bool> HandleForbiddenAsync(ChallengeContext context)
        {
            if (string.IsNullOrEmpty(_signatureOptions.UserAccessToken))
            {
                _errorCode = ErrorCode.MissingUserAccessToken;
            }

            var result = new WebApiResult(_errorCode, ErrorMessageFactory.GetErrorMessage(_errorCode));

            await Response.WriteAsync(JsonConvert.SerializeObject(result));

            return false;
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            var result = new WebApiResult(_errorCode, ErrorMessageFactory.GetErrorMessage(_errorCode));

            await Response.WriteAsync(JsonConvert.SerializeObject(result));

            return false;
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

