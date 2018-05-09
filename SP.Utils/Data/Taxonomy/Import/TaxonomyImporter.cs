using LumenWorks.Framework.IO.Csv;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using SP.Utils.Data.Taxonomy.Import.Collections;
using SP.Utils.Data.Taxonomy.Import.CSV;
using SP.Utils.DataStructures;
using SP.Utils.Extensioons;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Utils.Data.Taxonomy.Import
{
    public class TaxonomyImporter
    {
        const char DEFAULT_SEPARATOR = ';';
        const int DEFAULT_LCID = 1045;

        private Dictionary<string, SimpleTree<String>> Data;
        public bool OrderLikeInFile { get; set; }

        public string TermStoreName { get; private set; }
        public string TermStoreGroupName { get; private set; }

        public TaxonomyImporter(string termStoreName, string termStoreGroupName)
        {
            TermStoreName = termStoreName;
            TermStoreGroupName = termStoreGroupName;
        }

        public bool ImportCustomCsv(SPSite site, CsvParserBase parser, string csvFile, string termSetName, char separator = DEFAULT_SEPARATOR, int lcid = DEFAULT_LCID)
        {
            return ImportCustomCsv(site, parser, Encoding.UTF8, csvFile, termSetName, separator, lcid);
        }

        public bool ImportCustomCsv(TermStore oStore, CsvParserBase parser, string csvFile, string termSetName, char separator = DEFAULT_SEPARATOR, int lcid = DEFAULT_LCID)
        {
            return ImportCustomCsv(oStore, parser, Encoding.UTF8, csvFile, termSetName, separator, lcid);
        }

        public bool ImportCustomCsv(SPSite site, CsvParserBase parser, Encoding encoding, string csvFile, string termSetName, char separator = DEFAULT_SEPARATOR, int lcid = DEFAULT_LCID)
        {
            if (string.IsNullOrEmpty(csvFile))
                throw new ArgumentNullException("csvFile");

            if (string.IsNullOrEmpty(termSetName))
                throw new ArgumentNullException("importName");

            var result = parser.Parse(csvFile, Encoding.UTF8, true, separator);

            AddToCache(result, termSetName);

            return CreateStructureForTermSet(site, termSetName, lcid);
        }

        public bool ImportCustomCsv(TermStore oStore, CsvParserBase parser, Encoding encoding, string csvFile, string termSetName, char separator = DEFAULT_SEPARATOR, int lcid = DEFAULT_LCID)
        {
            if (string.IsNullOrEmpty(csvFile))
                throw new ArgumentNullException("csvFile");

            if (string.IsNullOrEmpty(termSetName))
                throw new ArgumentNullException("importName");

            var result = parser.Parse(csvFile, Encoding.UTF8, true, separator);

            AddToCache(result, termSetName);

            return CreateStructureForTermSet(oStore, termSetName, lcid);
        }

        public bool ImportEnum<T>(SPSite site, string termSetName, int lcid = 1045) where T : struct, IConvertible
        {
            StringCollection terms = EnumExtensions.GetBoundEnumCollection<T>();
            string[] arr = new string[terms.Count];
            terms.CopyTo(arr, 0);
            return ImportList(site, arr, termSetName, lcid);
        }

        public bool ImportEnum<T>(TermStore oStore, string termSetName, int lcid = 1045) where T : struct, IConvertible
        {
            StringCollection terms = EnumExtensions.GetBoundEnumCollection<T>();
            string[] arr = new string[terms.Count];
            terms.CopyTo(arr, 0);
            return ImportList(oStore, arr, termSetName, lcid);
        }

        public bool ImportList<T>(SPSite site, IEnumerable<T> terms, string termSetName, int lcid = 1045)
        {
            var parser = CollectionParser<T>.Create();
            var result = parser.Parse(terms);
            AddToCache(result, termSetName);

            return CreateStructureForTermSet(site, termSetName, lcid);
        }

        public bool ImportList<T>(TermStore oStore, IEnumerable<T> terms, string termSetName, int lcid = 1045)
        {
            var parser = CollectionParser<T>.Create();
            var result = parser.Parse(terms);
            AddToCache(result, termSetName);

            return CreateStructureForTermSet(oStore, termSetName, lcid);
        }

        public void ImportStandardCsv(TermStore oStore, string csvFile, Encoding encoding)
        {
            Group oGroup = oStore.Groups[TermStoreGroupName];

            ImportManager manager = oStore.GetImportManager();
            bool allAdded;
            string errorMessage;
            manager.ImportTermSet(oGroup, new StreamReader(csvFile, encoding), out allAdded, out errorMessage);
        }

        private bool CreateStructureForTermSet(SPSite site, string termSetName, int lcid = 1045)
        {
            TaxonomySession oSession = new TaxonomySession(site);
            TermStore oStore = oSession.TermStores[TermStoreName];
            return CreateStructureForTermSet(oStore, termSetName, lcid);
        }

        private bool CreateStructureForTermSet(TermStore oStore, string termSetName, int lcid = 1045)
        {
            Group oGroup = oStore.Groups[TermStoreGroupName];
            TermSet oTerms = oGroup.TermSets[termSetName];

            CreateOrUpdateTerms(oTerms, Data[termSetName], lcid);
            oStore.CommitAll();

            return true;
        }

        private void CreateOrUpdateTerms(TermSet oTerms, SimpleTree<string> data, int lcid)
        {
            if (oTerms == null)
                throw new ArgumentNullException("oTerms");
            if (data == null)
                throw new ArgumentNullException("data");

            var allNodes = SimpleTree<string>.Traverse(data, node => node).ToList();
            allNodes.ForEach(node => node.Clear());
            var currentNodes = TraverseTerms(new SimpleTree<string>("", oTerms.Name), oTerms, lcid, false).ToList();

            // stores terms and controls if they have uniqye key paths in store
            Dictionary<string, Term> termDict = new Dictionary<string, Term>();

            if (currentNodes.Count == 1)
            {
                // create all 
                foreach (SimpleTree<string> node in allNodes)
                {
                    if (oTerms.Name == node.Value)
                        continue;

                    int level = node.Key.Count(c => c == '.');
                    if (level == 1)
                    {
                        CreateAndAddToDict(oTerms, termDict, node, lcid);
                    }
                    else
                    {
                        CreateAndAddToDict(getParentTerm(node.Key, termDict), termDict, node, lcid);
                    }
                }
            }
            else
            {
                var toDisable = currentNodes.Except(allNodes).ToList();
                var toAdd = allNodes.Except(currentNodes).ToList();

                foreach (var node in toAdd)
                {
                    if (node.Parent.Value == oTerms.Name)
                    {
                        // check if disabled
                        var terms = oTerms.GetTerms(node.Value, lcid, false, StringMatchOption.ExactMatch, 1, false);
                        if (terms.Count > 0)
                        {
                            terms[0].IsAvailableForTagging = true;
                            termDict.Add(node.Key, terms[0]);
                        }
                        else
                        {
                            CreateAndAddToDict(oTerms, termDict, node, lcid);
                        }
                    }
                    else
                    {
                        // check if disabled
                        var terms = oTerms.GetTerms(node.Value, lcid, false, StringMatchOption.ExactMatch, 1, false);
                        if (terms.Count > 0)
                        {
                            terms[0].IsAvailableForTagging = true;
                            termDict.Add(node.Key, terms[0]);
                        }
                        else
                        {
                            // check in temp dict
                            if (termDict.ContainsKey(node.Parent.Key))
                            {
                                CreateAndAddToDict(getParentTerm(node.Key, termDict), termDict, node, lcid);
                            }
                            else
                            {
                                // get parent from site
                                terms = oTerms.GetTerms(node.Parent.Value, lcid, false, StringMatchOption.ExactMatch, 1, true);
                                if (terms.Count > 0)
                                {
                                    CreateAndAddToDict(terms[0], termDict, node, lcid);
                                }
                            }
                        }
                    }
                }
                foreach (var item in toDisable)
                {
                    var terms = oTerms.GetTerms(item.Value, lcid, false, StringMatchOption.ExactMatch, 1, true);
                    if (terms.Count > 0)
                    {
                        terms[0].IsAvailableForTagging = false;
                    }
                }
            }
        }

        private void CreateAndAddToDict(TermSetItem parent, Dictionary<string, Term> termDict, SimpleTree<string> node, int lcid)
        {
            Term t = parent.CreateTerm(node.Value, lcid);
            if (string.IsNullOrEmpty(node.Description) == false)
                t.SetDescription(node.Description, lcid);
            if (node.CustomProperties != null)
            {
                foreach (var termProperty in node.CustomProperties)
                {
                    t.SetCustomProperty(termProperty.Name, termProperty.Value);
                }
            }
            termDict.Add(node.Key, t);
        }

        private Term getParentTerm(string key, Dictionary<string, Term> parentTermsDict)
        {
            return parentTermsDict[getParentKey(key)];
        }

        private string getParentKey(string key)
        {
            int lastIndexOf = key.LastIndexOf('.');
            string parentKey = key.Substring(0, lastIndexOf);
            parentKey = parentKey.Substring(0, parentKey.LastIndexOf('.') + 1);
            return parentKey;
        }



        private void AddToCache(SimpleTree<string> result, string termSetName)
        {
            if (result != null)
            {
                result.Key = "";
                result.Value = termSetName;
                if (Data == null)
                    Data = new Dictionary<string, SimpleTree<string>>();
                Data.Add(termSetName, result);
            }
        }

        private static IEnumerable<SimpleTree<string>> TraverseTerms(SimpleTree<string> root, TermSet set, int lcid, bool traverseAll)
        {
            var stack = new Queue<SimpleTree<string>>();
            var termStack = new Queue<TermSetItem>();
            stack.Enqueue(root);
            termStack.Enqueue(set);
            while (stack.Any())
            {
                var next = stack.Dequeue();
                var nextTerm = termStack.Dequeue();
                yield return next;
                int index = 1;
                foreach (var child in nextTerm.Terms)
                {
                    if (child.IsAvailableForTagging || traverseAll)
                    {
                        termStack.Enqueue(child);
                        stack.Enqueue(new SimpleTree<string>(string.Format("{0}{1}.", next.Key, index), child.Name, child.GetDescription(lcid)) { Parent = next });
                        index++;
                    }
                }
            }
        }
    }
}
