using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.AD
{
    public class ActiveDirectoryClient
    {
        static readonly string USER_SEARCH_PATTERN = "(&(objectCategory=person)(objectClass=user)(|((cn=*{0}*)(displayName=*{0}*)(name=*{0}*)(SAMAccountName=*{0}*)(userPrincipalName=*{0}*))))";
        static readonly string USER_LOGIN_SEARCH_PATTERN = "(&((|((&((objectClass=user)(!(userAccountControl:1.2.840.113556.1.4.803:=2))))(objectClass=group)))(sAMAccountName={0})))";
        static readonly string GROUP_SEARCH_PATTERN = "(&(objectCategory=Group)(objectClass=Group)(|((name=*{0}*)(SAMAccountName=*{0}*)(cn=*{0}*))))";

        public ActiveDirectoryClient()
        {

        }

        public SearchResultCollection SearchUser(string str)
        {
            var searcher = GetDirectorySearcher();
            int num = str.TrimEnd(new char[] { '\\' }).IndexOf("\\");
            if (num >= 0)
            {
                str = str.Substring(num + 1);
            }
            searcher.Filter = string.Format(USER_SEARCH_PATTERN, str);
            return searcher.FindAll();
        }

        public SearchResultCollection SearchUserByLogin(string login)
        {
            var searcher = GetDirectorySearcher();
            int num = login.TrimEnd(new char[] { '\\' }).IndexOf("\\");
            if (num >= 0)
            {
                login = login.Substring(num + 1);
            }
            searcher.Filter = string.Format(USER_LOGIN_SEARCH_PATTERN, login);
            return searcher.FindAll();
        }

        public SearchResultCollection SearchGroup(string str)
        {
            var searcher = GetDirectorySearcher();
            int num = str.TrimEnd(new char[] { '\\' }).IndexOf("\\");
            if (num >= 0)
            {
                str = str.Substring(num + 1);
            }
            searcher.Filter = string.Format(GROUP_SEARCH_PATTERN, str);
            return searcher.FindAll();
        }

        public string GetPropertyValue(string userLogin, string propertyName)
        {
            throw new NotImplementedException();
        }

        private DirectoryEntry GetDirectoryEntry()
        {
            Domain domain = Domain.GetCurrentDomain();
            var de = domain.GetDirectoryEntry();
            de.AuthenticationType = AuthenticationTypes.Secure;
            return de;
        }

        private DirectorySearcher GetDirectorySearcher()
        {
            var de = GetDirectoryEntry();
            var searcher = new DirectorySearcher();
            searcher.SearchRoot = de;
            return searcher;
        }
    }
}
