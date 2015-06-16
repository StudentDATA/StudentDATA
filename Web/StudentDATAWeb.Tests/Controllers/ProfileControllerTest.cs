using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StudentDATAWeb.Controllers;
using System.Web.Mvc;
using StudentDATAWeb.Models;
using System.Data.Entity;


namespace StudentDATAWeb.Tests.Controllers
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class ProfileControllerTest
    {
        ProfileController pc;
        public ProfileControllerTest()
        {
            pc = new ProfileController();
            //
            // TODO: ajoutez ici la logique du constructeur
            //
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
        public void CodeCutterTest()
        {

            Assert.IsNull(pc.CodeCutter(null));
            List<string> codecutterList = new List<string>();
            codecutterList = pc.CodeCutter("S05IL");
            Assert.IsNotNull(codecutterList);
            Assert.IsTrue(codecutterList[0] == "05");
            Assert.IsTrue(codecutterList[1] == "IL");
            codecutterList.Clear();
            codecutterList = pc.CodeCutter("ahblblblblblbl");
            Assert.IsTrue(codecutterList[1] == null);
            codecutterList.Clear();
            codecutterList = pc.CodeCutter("S00PO");
            Assert.IsTrue(codecutterList[0] == "00");
            Assert.IsTrue(codecutterList[1] == "pedago");

        }
        [TestMethod]
        public void CodeCreatorTest()
        {
            List<string> tmpList = new List<string>();
            tmpList.Add("05");
            tmpList.Add("IL");
            Assert.IsNotNull(pc.CodeCreator(tmpList));
            Assert.IsTrue(pc.CodeCreator(tmpList) == "S05IL");
            tmpList.Clear();
            tmpList = null;
            Assert.IsNull(pc.CodeCreator(tmpList));
            tmpList = new List<string>();
            tmpList.Add("00");
            tmpList.Add("pedago");
            Assert.IsTrue(pc.CodeCreator(tmpList) == "S00PO");
            tmpList.Clear();
            tmpList.Add("01");
            tmpList.Add("Common");
            Assert.IsTrue(pc.CodeCreator(tmpList) == "S01TC");
        }

     
    }
}
