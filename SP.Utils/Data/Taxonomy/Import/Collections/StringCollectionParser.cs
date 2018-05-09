using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SP.Utils.DataStructures;

namespace SP.Utils.Data.Taxonomy.Import.Collections
{
    public class StringCollectionParser : CollectionParser<String>
    {
        public override SimpleTree<string> Parse(IEnumerable<string> items)
        {
            SimpleTree<string> result = new SimpleTree<string>();

            int index = 1;
            foreach (var item in items)
            {
                string path = string.Format("{0}.", index);
                string name = item;

                AddToTree(result, path, name);
                index++;
            }

            return result;
        }
    }
}
