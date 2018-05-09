using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SP.Tests
{
    public class LinearTest
    {
        private static readonly object SyncRoot = new object();

        [TestInitialize]
        public void Initialize()
        {
            Monitor.Enter(SyncRoot);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Monitor.Exit(SyncRoot);
        }
    }
}
