using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Newtonsoft.Json;
using zxm.AspNetCore.Authentication.Hmac.Identity;
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

            var requestDateTime = GetTime(_signatureOptions.Timestamp);
            if (requestDateTime == null)
            {
                _errorCode = ErrorCode.InvalidTimestamp;
                return AuthenticateResult.Fail("Invalid timestamp.");
            }
            else
            {
                if ((DateTime.Now - requestDateTime).Value.TotalSeconds > Options.RequestTimeInterval)
                {
                    _errorCode = ErrorCode.TimestampExpired;
                    return AuthenticateResult.Fail("request timestamp expired.");
                }
            }

            var signature = SignatureFactory.GenerateSignature(_signatureOptions);
            if (!_signatureInRequest.Equals(signature, StringComparison.OrdinalIgnoreCase))
            {
                _errorCode = ErrorCode.InvalidSignature;
                return AuthenticateResult.Fail("Invalid signature.");
            }

            HmacIdentity identityUser = null;
            if (!string.IsNullOrEmpty(_signatureOptions.UserAccessToken) && Options.VerifyUserAccessToken != null)
            {
                identityUser = Options.VerifyUserAccessToken(_signatureOptions.UserAccessToken);
                if (identityUser == null)
                {
                    _errorCode = ErrorCode.InvalidUserAccessToken;
                }
            }
            if (identityUser == null)
            {
                identityUser = new HmacIdentity(_signatureOptions.ClientId);
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

            _signatureOptions.Timestamp = Request.Query[SignatureKeys.Timestamp];
            if (string.IsNullOrEmpty(_signatureOptions.Timestamp))
            {
                _errorCode = ErrorCode.MissingTimestamp;
                return false;
            }

            _signatureOptions.UserAccessToken = Request.Query[SignatureKeys.UserAccessToken];

            using (var streamReader = new StreamReader(Request.Body))
            {
                _signatureOptions.PostData = streamReader.ReadToEnd();
            }

            return true;
        }

        private DateTime? GetTime(string timeStamp)
        {
            DateTime dtStart;
#if COREFX
            dtStart = new DateTime(1970, 1, 1);
#else
            dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970,1,1)); 
#endif
            long lTime;
            if (long.TryParse(timeStamp + "0000000", out lTime))
            {
                var toNow = new TimeSpan(lTime);
                return dtStart.Add(toNow);
            }
            else
            {
                return null;
            }
        }

        protected override async Task<bool> HandleForbiddenAsync(ChallengeContext context)
        {
            if (string.IsNullOrEmpty(_signatureOptions.UserAccessToken))
            {
                _errorCode = ErrorCode.MissingUserAccessToken;
            }

            var result = new WebApiResult(_errorCode, ErrorMessageDefaults.GetMessage(_errorCode));

            await Response.WriteAsync(JsonConvert.SerializeObject(result));

            return false;
        }

        protected override async Task<bool> HandleUnauthorizedAsync(ChallengeContext context)
        {
            var result = new WebApiResult(_errorCode, ErrorMessageDefaults.GetMessage(_errorCode));

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

