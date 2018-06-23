using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using XGamer.Core;
using XGamer.Data.Entities;

namespace XGamer.Core.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class XGamerEngineTest
    {
        public XGamerEngineTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        //[TestMethod]
        //[HostType("Moles")]
        //public void GetAllGamesShouldReturnsData()
        //{
        //    MEFDataProvider.AllInstances.GetAllGames = (dp) => { return new List<Game>() { new Game(), new Game()}; };
        //    MImageCache.ConstructorString = (ic, s) => { };
        //    MImageCache.AllInstances.ProcessImageFolder = (ic) => { };

        //    XGamerEngine xgameEngine = XGamerEngine.Instance;

        //    IEnumerable<Game> allGames = xgameEngine.GetAllGames();

        //    Assert.IsNotNull(allGames);
        //    Assert.AreEqual(2, allGames.Count());
        //}
    }
}
