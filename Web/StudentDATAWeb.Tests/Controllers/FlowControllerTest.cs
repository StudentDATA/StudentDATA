using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentDATAWeb.Controllers;
using StudentDATAWeb.Models;
using System.IO;
using System.Data.Entity;
using System.Web.Mvc;

namespace StudentDATAWeb.Tests.Controllers
{
    /// <summary>
    /// Description résumée pour FlowControllerTest
    /// </summary>
    [TestClass]
    public class FlowControllerTest
    {
        HomeController hc;
        FlowController fc;
        public FlowControllerTest()
        {
            hc = new HomeController();
            ViewResult vr = hc.Index() as ViewResult;
            //fc = new FlowController();
            //fc.Index(new UsersContext());
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active ainsi que ses fonctionnalités.
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

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void RSSInitializerDBTest()
        {
            fc.InitializeRSSFlowsDatas();
            for (int i = 0; i <= 10; i++)
            {
                if (i < 10)
                    Assert.IsTrue(System.IO.File.Exists("/Content/RSSXML/" + "S0" + i.ToString() + ".xml"));
                else
                    Assert.IsTrue(System.IO.File.Exists("/Content/RSSXML/" + "S" + i.ToString() + ".xml"));
            }
            Assert.IsTrue(System.IO.File.Exists("/Content/RSSXML/IL.xml"));
            Assert.IsTrue(System.IO.File.Exists("/Content/RSSXML/SR.xml"));
            Assert.IsTrue(System.IO.File.Exists("/Content/RSSXML/pedago.xml"));
            Assert.IsTrue(System.IO.File.Exists("/Content/RSSXML/TC.xml"));
        }
    }
}
