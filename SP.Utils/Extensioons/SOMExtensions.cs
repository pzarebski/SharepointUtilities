﻿using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public static void PrepareMembershipForCurrentUserQuery(this SPQuery query, string fieldName)
        {
            query.Query = string.Format("<Or><Membership Type='CurrentUserGroups'><FieldRef Name='{0}'/></Membership><Eq><FieldRef Name='{0}'></FieldRef><Value Type='Integer'><UserID/></Value></Eq></Or>", fieldName);
        }

        public static void PrepareInWhere(this SPQuery query, string fieldInternalName, SPFieldType type, List<string> lstValues)
        {
            new StringBuilder();
            XElement whereElement = new XElement(XName.Get("Where"));
            XElement inElement = new XElement(XName.Get("In"));
            XElement content = XElement.Parse(string.Format("<FieldRef Name='{0}' />", fieldInternalName));
            XElement valuesElement = new XElement(XName.Get("Values"));
            inElement.Add(content);
            lstValues.Add("0");
            string text = string.Format("<Value Type='{0}' />", type);
            foreach (string current in lstValues)
            {
                XElement valueElement = XElement.Parse(text);
                valueElement.Value = current;
                valuesElement.Add(valueElement);
            }
            inElement.Add(valuesElement);
            whereElement.Add(inElement);
            query.Query = whereElement.ToString();
        }

        public static void PrepareViewFields(this SPQuery query, params string[] fieldNames)
        {
            StringBuilder stringBuilder = new StringBuilder(50);
            for (int i = 0; i < fieldNames.Length; i++)
            {
                string arg = fieldNames[i];
                stringBuilder.AppendFormat("<FieldRef Name=\"{0}\" />", arg);
            }
            query.ViewFields =  stringBuilder.ToString();
        }
    }
}
