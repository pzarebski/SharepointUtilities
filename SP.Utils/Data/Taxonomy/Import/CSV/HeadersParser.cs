using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public class HeadersParser : CsvParserBase
    {
        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            while (csv.ReadNextRecord())
            {
                string header = csv["Header"];

                string path = header.Split('-')[0].Trim() + ".";
                string name = header;

                AddToTree(result, path, name);
            }

            return result;
        }
    }
}
