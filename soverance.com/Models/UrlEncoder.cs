using System;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;

namespace soverance.com.Models
{
    public static class UrlEncoder
    {
        public static string SanitizeUrl(string urlToEncode)
        {
            urlToEncode = (urlToEncode ?? "").Trim().ToLower();

            StringBuilder url = new StringBuilder();

            foreach (char ch in urlToEncode)
            {
                switch (ch)
                {
                    // replace invalid characters
                    case ' ':
                        url.Append('-');
                        break;
                    case '&':
                        url.Append("and");
                        break;
                    case '\'':
                        break;
                    // replace spaces with hyphens, and keep all other characters
                    default:
                        if ((ch >= '0' && ch <= '9') || (ch >= 'a' && ch <= 'z'))
                        {
                            url.Append(ch);
                        }
                        else
                        {
                            url.Append('-');
                        }
                        break;
                }
            }

            return url.ToString();
        }
    }
}
