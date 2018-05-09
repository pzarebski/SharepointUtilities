using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Extensioons
{
    public static class SOMExtensions
    {
        public static string ToStringSafe(this SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return String.Empty;

            return item[fieldName].ToString().Trim();
        }

        public static int ToInt32Safe(this SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return 0;

            int integer = 0;
            if (Int32.TryParse((item[fieldName] ?? "").ToString(), out integer))
                return integer;
            return 0;
        }

        public static decimal ToDecimal(this SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return 0;

            decimal dec = 0;
            if (Decimal.TryParse((item[fieldName] ?? "").ToString().Replace(',', '.'), out dec))
                return dec;
            return 0;
        }

        public static DateTime ToDateTime(this SPListItem item, string fieldName)
        {
            if (item[fieldName] == null)
                return DateTime.MinValue;

            DateTime dt = DateTime.MinValue;
            if (DateTime.TryParse((item[fieldName] ?? "").ToString(), out dt))
                return dt;
            return DateTime.MinValue;
        }

        public static string SiteRelativeURL(this SPWeb web)
        {
            string webRelativeUrl = web.ServerRelativeUrl;
            if (!webRelativeUrl.EndsWith("/"))
                webRelativeUrl += "/";

            return webRelativeUrl;
        }

        public static string GetLibraryRelativeURL(this SPWeb web, string libraryURL)
        {
            return string.Format("{0}{1}", web.SiteRelativeURL(), libraryURL);
        }

        public static string GetListRelativeURL(this SPWeb web, string listURL)
        {
            return string.Format("{0}Lists/{1}", web.SiteRelativeURL(), listURL);
        }

    }
}
