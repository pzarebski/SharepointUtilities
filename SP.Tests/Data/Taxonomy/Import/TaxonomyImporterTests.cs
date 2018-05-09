using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SP.Tests;
using SP.Tests.Properties;
using SP.Utils.Data.Taxonomy.Import;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SP.Utils.Data.Taxonomy.Import.Tests
{
    /// <summary>
    /// debug not signed DLL's
    /// If test are not showing try restarting VS
    /// If test are not showing check your proc arch in Menu -> Test -> Test Setting -> Default Processor Architecture
    /// </summary>
    [TestClass()]
    public class TaxonomyImporterTests : LinearTest
    {
        SPWeb _web;
        SPSite _site;

        string termStoreName = "Managed Metadata Service";
        string termStoreGroupName = "Test";

        [TestInitialize]
        public void TestInitialize()
        {
            _site = new SPSite(Settings.Default.DEV_SITE);
            _web = _site.OpenWeb();

            // Ensure HttpContext.Current
            if (HttpContext.Current == null)
            {                
                HttpRequest request = new HttpRequest("", _web.Url, "");
                HttpContext.Current = new HttpContext(request,
                    new HttpResponse(TextWriter.Null));
                // TaxonomySession class uses utility function which checks HttpContext.Current.User for current user
                HttpContext.Current.User = System.Threading.Thread.CurrentPrincipal;
            }

            // SPContext is based on SPControl.GetContextWeb(), which looks here
            if (HttpContext.Current.Items["HttpHandlerSPWeb"] == null)
                HttpContext.Current.Items["HttpHandlerSPWeb"] = _web;            
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            if (_web != null)
                _web.Dispose();
            if (_site != null)
                _site.Dispose();
            HttpContext.Current = null;
        }

        [TestMethod()]
        public void TaxonomyImporterTest()
        {
            var importer = new TaxonomyImporter(termStoreName, termStoreGroupName);
            Assert.IsNotNull(importer);
            Assert.AreEqual(termStoreName, importer.TermStoreName);
            Assert.AreEqual(termStoreGroupName, importer.TermStoreGroupName);
        }

        [TestMethod()]
        public void ImportCustomCsvTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ImportCustomCsvTest1()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ImportCustomCsvTest2()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ImportCustomCsvTest3()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ImportEnumTest()
        {
            var importer = new TaxonomyImporter(termStoreName, termStoreGroupName);
            
            bool result = importer.ImportEnum<TestEnum>(_site, "TestEnum", 1033);
            
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void ImportListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ImportStandardCsvTest()
        {
            Assert.Fail();
        }
    }

    public enum TestEnum
    {
        Val1,
        Val2
    }
}