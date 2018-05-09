using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public class NamesParser : CsvParserBase
    {
        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            int index = 1;
            while (csv.ReadNextRecord())
            {
                string path = string.Format("{0}.", index);
                string name = csv["Name"];

                AddToTree(result, path, name);
                index++;
            }

            return result;
        }
    }
}
