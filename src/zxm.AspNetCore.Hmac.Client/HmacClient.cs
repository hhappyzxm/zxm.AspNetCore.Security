using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using zxm.AspNetCore.Authentication.Hmac.Signature;
using zxm.AspNetCore.WebApi.Result.Abstractions;

namespace zxm.AspNetCore.Hmac.Client
{
    public static class HmacClient
    {
        public static Task<HmacResponseMessage> PostAsync(string uri, string clientId, string clientSecret,
            string userAccessToken = null, object postData = null)
        {
            var jsonData = JsonConvert.SerializeObject(postData);

            return PostAsync(uri, clientId, clientSecret, userAccessToken, jsonData);
        }

        public static async Task<HmacResponseMessage> PostAsync(string uri, string clientId, string clientSecret, string userAccessToken, string postData)
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
                PostData = postData
            };

            uri += $"?{BuildQueryString(options)}";

            using (var httpClient = new HttpClient())
            {
                HttpContent httpContent = new StringContent(options.PostData);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response =  await httpClient.PostAsync(uri, httpContent);
                IWebApiResult result = null;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    if (content != null)
                    {
                        result = JsonConvert.DeserializeObject<WebApiResult>(content);
                    }
                }

                return new HmacResponseMessage
                {
                    ResponseMessage = response,
                    Result = result
                };
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

            return ((int)(time - startTime).TotalSeconds).ToString();
        }
    }
}
