using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SP.Utils.Extensioons
{
    public static class CoreExtensions
    {
        public static bool IsAZaz(this string s1)
        {
            string s2 = Regex.Replace(s1, @"[^A-Za-z]", string.Empty);
            return s1 == s2;
        }
        
        public static bool IsAZaz09(this string s1)
        {
            string s2 = Regex.Replace(s1, @"[^A-Za-z\d]", string.Empty);
            return s1 == s2;
        }
    }
}
