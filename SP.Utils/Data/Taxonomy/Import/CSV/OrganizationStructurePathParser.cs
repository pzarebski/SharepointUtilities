using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public class OrganizationStructurePathParser : CsvParserBase
    {
        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            while (csv.ReadNextRecord())
            {
                string pathValue = csv["Path"];

                string path = pathValue.Split('-')[0].Trim() + ".";
                string name = string.Empty;
                if (pathValue.IndexOf('.') >= 0)
                    if (pathValue.LastIndexOf('.') < pathValue.IndexOf('-'))
                        name = pathValue.Substring(pathValue.LastIndexOf('.') + 1);
                    else
                    {
                        string temp = pathValue.Substring(0, pathValue.IndexOf('-'));
                        name = temp.Substring(temp.LastIndexOf('.') + 1) + pathValue.Substring(pathValue.IndexOf('-'));
                    }
                else
                    name = pathValue;

                AddToTree(result, path, name);
            }

            return result;
        }
    }
}
