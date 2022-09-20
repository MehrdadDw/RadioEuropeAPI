using System;
using System.Text;

namespace RadioEurope.Utilities
{
    public static class EncodingExtensions
    {
        /// <summary>
        /// Method <c>EncodeBase64</c> Encodes value to base64.
        /// </summary>
        public static string EncodeBase64(this string value)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(valueBytes);
        }
        /// <summary>
        /// Method <c>DecodeBase64</c> Decodes base64 encoded value.
        /// </summary>
        public static string DecodeBase64(this string value)
        {
            value=value.Replace("\\\"","");
            var valueBytes = System.Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(valueBytes);
        }
    }
}