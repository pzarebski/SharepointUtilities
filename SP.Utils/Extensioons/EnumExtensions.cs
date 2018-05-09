using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Extensioons
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            var descriptions = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            return descriptions.Length > 0 ? descriptions[0].Description : value.ToString();
        }

        public static T GetValue<T>(string description) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an Enum type.");

            T result = default(T);

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.FieldType.Equals(typeof(T)))
                {
                    var value = (T)fi.GetValue(fi);
                    var desc = GetDescription((Enum)fi.GetValue(fi));

                    if (desc == description)
                    {
                        result = value;
                        break;
                    }
                }
            }

            return result;
        }

        public static string[] GetBoundEnumDescriptions<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an Enum type.");

            List<string> results = new List<string>();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.FieldType.Equals(typeof(T)))
                {
                    var value = (T)fi.GetValue(fi);
                    var description = GetDescription((Enum)fi.GetValue(fi));

                    if (!results.Contains(description))
                    {
                        results.Add(description);
                    }
                }
            }

            return results.ToArray();
        }

        public static StringCollection GetBoundEnumCollection<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an Enum type.");

            StringCollection results = new StringCollection();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.FieldType.Equals(typeof(T)))
                {
                    var value = (T)fi.GetValue(fi);
                    var description = GetDescription((Enum)fi.GetValue(fi));

                    if (!results.Contains(description))
                    {
                        results.Add(description);
                    }
                }
            }

            return results;
        }

        public static SortedDictionary<string, T> GetBoundEnumDictionary<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an Enum type.");

            SortedDictionary<string, T> results = new SortedDictionary<string, T>();

            FieldInfo[] fieldInfos = typeof(T).GetFields();
            foreach (FieldInfo fi in fieldInfos)
            {
                if (fi.FieldType.Equals(typeof(T)))
                {
                    var value = (T)fi.GetValue(fi);
                    var description = GetDescription((Enum)fi.GetValue(fi));

                    if (!results.ContainsKey(description))
                    {
                        results.Add(description, value);
                    }
                }
            }

            return results;
        }
    }
}
