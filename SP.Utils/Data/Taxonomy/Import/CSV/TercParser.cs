using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    /// <summary>
    /// https://pl.wikipedia.org/wiki/TERC
    /// </summary>
    public class TercParser : CsvParserBase
    {
        public TercParser() : base() { }

        protected override SimpleTree<string> ParseCsv(CsvReader csv)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            string temp = null;
            while (csv.ReadNextRecord())
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(csv["WOJ"]);
                builder.Append('.');
                if (string.IsNullOrEmpty((temp = csv["POW"])) == false)
                {
                    builder.Append(temp);
                    builder.Append('.');
                }
                if (string.IsNullOrEmpty((temp = csv["GMI"])) == false)
                {
                    builder.Append(temp);
                    builder.Append(csv["RODZ"]);
                    builder.Append('.');
                }

                string path = builder.ToString();
                string name = string.Format("{0} ({1})", csv["NAZWA"], csv["NAZDOD"]);

                AddToTree(result, path, name);
            }

            return result;
        }
    }
}
