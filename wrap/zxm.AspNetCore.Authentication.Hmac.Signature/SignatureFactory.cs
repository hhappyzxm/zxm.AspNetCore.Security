using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace zxm.AspNetCore.Authentication.Hmac.Signature
{
    public static class SignatureFactory
    {
        public static string GenerateSignature(SignatureOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (string.IsNullOrEmpty(options.ClientId))
            {
                throw new ArgumentNullException(nameof(options.ClientId));
            }

            if (string.IsNullOrEmpty(options.ClientSecret))
            {
                throw new ArgumentNullException(nameof(options.ClientSecret));
            }

            if (string.IsNullOrEmpty(options.ClientSecret))
            {
                throw new ArgumentNullException(nameof(options.ClientSecret));
            }

            var sortedDictionary = GetSortedDictionary(options);

            var encryptionStr = GetEncryptionString(sortedDictionary);

            return GetMd5_32(encryptionStr, options.Encoding);
        }

        private static SortedDictionary<string, string> GetSortedDictionary(SignatureOptions options)
        {
            var dic = new SortedDictionary<string, string>
            {
                {SignatureKeys.ClientId.ToLower(), options.ClientId},
                {SignatureKeys.ClientSecret.ToLower(), options.ClientSecret},
                {SignatureKeys.Timestamp.ToLower(), options.Timestamp.ToString()}
            };
            if (!string.IsNullOrEmpty(options.AccessToken)) dic.Add(SignatureKeys.AccessToken, options.AccessToken);
            if (!string.IsNullOrEmpty(options.PostData)) dic.Add(SignatureKeys.PostData, options.PostData);

            return dic;
        }

        private static string GetEncryptionString(SortedDictionary<string, string> dic)
        {
            var sb = new StringBuilder();
            foreach (var kv in dic)
            {
                sb.Append('&');
                sb.Append(kv.Key);
                sb.Append('=');
                sb.Append(kv.Value);
            }
            sb.Remove(0, 1);

            return sb.ToString();
        }

        public static string GetMd5_32(this string encryptionStr, Encoding encoding)
        {
            byte[] data = GetMd5(encryptionStr, encoding);
            var tmp = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                tmp.Append(data[i].ToString("x2"));
            }
            return tmp.ToString();
        }

        private static byte[] GetMd5(string encryptionStr, Encoding encoding)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                return md5Hash.ComputeHash(encoding.GetBytes(encryptionStr));
            }
        }
    }
}
