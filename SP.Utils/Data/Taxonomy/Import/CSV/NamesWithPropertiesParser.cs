using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public class NamesWithPropertiesParser : CsvParserBase
    {
        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            int index = 1;
            while (csv.ReadNextRecord())
            {
                string path = string.Format("{0}.", index);
                string name = string.Format("{0} - {1}", csv["Path"], csv["Name"]);
                string desc = csv["Desc"];

                AddToTree(result, path, name, desc);
                index++;
            }

            return result;
        }
    }
}
