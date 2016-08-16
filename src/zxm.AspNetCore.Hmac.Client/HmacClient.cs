using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using zxm.AspNetCore.Authentication.Hmac.Signature;
using zxm.AspNetCore.WebApi.Result.Abstractions;

namespace zxm.AspNetCore.Hmac.Client
{
    public static class HmacClient
    {
        public static Task<IWebApiResult> PostAsync(string uri, string clientId, string clientSecret, string userAccessToken = null, object postData = null)
        {
            var jsonData = string.Empty;
            if (postData != null)
            {
                jsonData = JsonConvert.SerializeObject(postData);
            }

            return PostAsync(uri, clientId, clientSecret, userAccessToken, jsonData);
        }

        public static async Task<IWebApiResult> PostAsync(string uri, string clientId, string clientSecret, string userAccessToken = null, string postData = null)
        {
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentNullException(nameof(uri));
            }

            if (string.IsNullOrEmpty(clientId))
            {
                throw new ArgumentNullException(nameof(clientId));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentNullException(nameof(clientSecret));
            }

            var options = new SignatureOptions
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                Timestamp = GetCurrentTimestamp(DateTime.Now),
                UserAccessToken = userAccessToken,
            };

            uri += $"?{BuildQueryString(options)}";

            using (var httpClient = new HttpClient())
            {
                HttpContent htttpContent = null;
                if (string.IsNullOrEmpty(postData))
                {
                    htttpContent = new StringContent(options.PostData, Encoding.UTF8);
                }

                var response = await httpClient.PostAsync(uri, htttpContent);
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IWebApiResult>(result);
            }
        }

        private static string BuildQueryString(SignatureOptions options)
        {
            var signature = SignatureFactory.GenerateSignature(options);

            var queryString =
                $"{SignatureKeys.ClientId.ToLower()}={options.ClientId}&{SignatureKeys.Timestamp.ToLower()}={options.Timestamp}&{SignatureKeys.Signature.ToLower()}={signature}";

            if (!string.IsNullOrEmpty(options.UserAccessToken))
            {
                queryString += $"&{SignatureKeys.UserAccessToken}={options.UserAccessToken}";
            }

            return queryString;
        }

        private static string GetCurrentTimestamp(DateTime time)
        {
            DateTime startTime;
#if COREFX
            startTime = new DateTime(1970, 1, 1);
#else
            startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
#endif

            return (time - startTime).TotalSeconds.ToString();
        }
    }
}
