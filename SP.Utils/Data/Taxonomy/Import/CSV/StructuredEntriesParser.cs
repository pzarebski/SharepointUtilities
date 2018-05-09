using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public class StructuredEntriesParser : CsvParserBase
    {
        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            while (csv.ReadNextRecord())
            {
                string path = csv["Path"];
                string name = csv["Name"];
                name = string.Format("{0} {1}", path.Trim(), name.Trim());

                AddToTree(result, path, name);
            }

            return result;
        }
    }
}
