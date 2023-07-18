using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Web.Core.Util
{
    public class DataHelper
    {
        public static string SHA256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public static string Unsign(string value)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = value.Normalize(NormalizationForm.FormD);
            string str1 = regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
            return Regex.Replace(str1, @"[^0-9a-zA-Z]+", "-").ToLower();
        }

        public static string ToCurrency(double? value)
        {
            return string.Format("{0:#,##0}", value).Replace(",", ".");
        }
    }
}
