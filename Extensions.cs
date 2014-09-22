using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DirLink
{
    public static class Extensions
    {
        /// <summary>
        ///     Escapes the LDAP search filter to prevent LDAP injection attacks.
        /// </summary>
        /// <param name="searchFilter">The search filter.</param>
        /// <see cref="http://blogs.sun.com/shankar/entry/what_is_ldap_injection" />
        /// <see cref="http://msdn.microsoft.com/en-us/library/aa746475.aspx" />
        /// <returns>The escaped search filter.</returns>
        public static string EscapeLdapSearchFilter(this string searchFilter)
        {
            // From: http://stackoverflow.com/questions/649149/how-to-escape-a-string-in-c-for-use-in-an-ldap-query/694915#694915

            if (searchFilter == null) throw new ArgumentNullException("searchFilter");

            var escape = new StringBuilder();

            foreach (char c in searchFilter)
            {
                switch (c)
                {
                    case '\\':
                        escape.Append(@"\5c");
                        break;
                    case '*':
                        escape.Append(@"\2a");
                        break;
                    case '(':
                        escape.Append(@"\28");
                        break;
                    case ')':
                        escape.Append(@"\29");
                        break;
                    case '\u0000':
                        escape.Append(@"\00");
                        break;
                    case '/':
                        escape.Append(@"\2f");
                        break;
                    default:
                        escape.Append(c);
                        break;
                }
            }

            return escape.ToString();
        }
    }
}