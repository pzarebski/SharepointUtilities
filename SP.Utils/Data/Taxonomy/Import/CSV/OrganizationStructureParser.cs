using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public class OrganizationStructureParser : CsvParserBase
    {
        const string DEPARTMENT_PROP_NAME = "Unit";

        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            while (csv.ReadNextRecord())
            {
                string unit = string.Empty;
                string[] arr = new string[Headers.Length];
                csv.CopyCurrentRecordTo(arr);
                StringBuilder org = new StringBuilder();
                for (int i = 0; i < Headers.Length - 2; i++)
                {
                    if (string.IsNullOrEmpty(arr[i]) == false)
                    {
                        org.Append(arr[i]);
                        org.Append('.');
                        unit = Headers[i];
                    }
                }

                string path = org.ToString();
                string name = string.Format("{0} - {1}", path.TrimEnd('.').Substring(path.TrimEnd('.').LastIndexOf('.') + 1), arr[6]);
                string desc = arr[7];

                AddToTree(result, path, name, desc, new Property(DEPARTMENT_PROP_NAME, unit));
            }

            return result;
        }
    }
}
