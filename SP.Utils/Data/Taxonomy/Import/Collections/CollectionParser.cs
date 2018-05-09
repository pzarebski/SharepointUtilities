using SP.Utils.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Data.Taxonomy.Import.Collections
{
    public abstract class CollectionParser<T> : ParserBase
    {
        public abstract SimpleTree<string> Parse(IEnumerable<T> items);

        public static CollectionParser<T> Create()
        {
            if (typeof(T) == typeof(string))
                return new StringCollectionParser() as CollectionParser<T>;

            throw new NotImplementedException("Factory method for Type: " + typeof(T).Name + " is not implemented.");
        }
    }
}
