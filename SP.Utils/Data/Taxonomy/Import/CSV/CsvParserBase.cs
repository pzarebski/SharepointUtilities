using LumenWorks.Framework.IO.Csv;
using SP.Utils.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Data.Taxonomy.Import.CSV
{
    public abstract class CsvParserBase : ParserBase
    {
        protected string[] Headers { get; private set; }

        public CsvParserBase() : base()
        {

        }

        protected abstract SimpleTree<string> ParseCsv(CsvReader csv);

        public SimpleTree<string> Parse(string csvFilePath)
        {
            return Parse(csvFilePath, Encoding.UTF8, true, ';');
        }

        public SimpleTree<string> Parse(string csvFilePath, Encoding encoding, bool hasHeader, char separator)
        {
            using (CsvReader csv = new CsvReader(new StreamReader(csvFilePath, encoding), hasHeader, separator))
            {
                Headers = csv.GetFieldHeaders();

                return ParseCsv(csv);
            }
        }

        
    }
}
