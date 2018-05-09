using SP.Utils.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Data.Taxonomy.Import
{
    public abstract class ParserBase
    {
        protected void AddToTree(SimpleTree<string> tree, string path, string name, string desc = null, params Property[] cproperties)
        {
            var parent = getEntryParent(tree, path);
            if (parent != null)
            {
                parent.Add(new SimpleTree<string>(path.Trim(), name.Trim(), desc, cproperties));
            }
            else
            {
                throw new Exception(string.Format("Nie dodano jeszcze elementu nadrzędnego dla '{0}'.", path.Trim()));
            }
        }

        private SimpleTree<string> getEntryParent(SimpleTree<string> root, string path)
        {
            int index = 0;
            int lastIndex = path.LastIndexOf('.');
            var parent = root;
            while ((index = path.IndexOf('.', index + 1)) < lastIndex)
            {
                parent = parent.Where(v => v.Key == path.Substring(0, index + 1)).SingleOrDefault();
            }
            return parent;
        }
    }
}
