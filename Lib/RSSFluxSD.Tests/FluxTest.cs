using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSSFluxSD;
using System.Threading;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace RSSFluxSD.Tests
{
	[TestClass]
	public class FluxTest
	{

		[TestMethod]
		public void TestReadWithManage()
		{
			//Faire methode pour lire une seule et unique rss
			RSSManage rssM = new RSSManage();
			//Verfier si on a pas internet;
			//RSS rss1 = rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			//rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			//rssM.readRSS("http://www.developpez.com/index/rss");

			RSS rss = rssM.readRSS("testAdd.xml");

			Console.WriteLine(rss.Tilte());
			foreach ( Flow flow in rss.GetAllFlow())
			{
				Console.WriteLine(flow.Title);
				Console.WriteLine(flow.Content);
			}

			List<Flow> ListFlow = new List<Flow>();
			ListFlow.Add(new Flow("Test7", "Le test 7"));
			rss.AddFlow(ListFlow);
			Console.WriteLine(rssM.GetAllRSS().Count);
			//Assert.AreEqual(rssM.GetAllRSS().Count, 2);
			foreach( RSS rsst in rssM.GetAllRSS())
			{
				Console.WriteLine(rsst.GetAllFlow().Count);
			}
			Console.WriteLine();

			HelpTest.HelpReadWithManage(rssM);

		}

		[TestMethod]
		public void TestCreateAndAddXML()
		{
			RSSManage rssM = new RSSManage();
			string path = "test.xml";
			RSS rss = rssM.createRSS(path, "Test", "C'est un test", Helper.CategorieRSSEnum.Etudiant);
			RSS rss1 = rssM.createRSS("https://fr.news.yahoo.com/rss/world", "Test", "C'est un test", Helper.CategorieRSSEnum.Etudiant);

			if ( rssM.Msg_error != null)
			{
				Console.WriteLine(rssM.Msg_error);
			}
			rss.Save(Helper.FormatRSSEnum.RSS20);
			HelpTest.HelpReadWithManage(rssM);

			Assert.AreEqual(rss.Uri_RSS, "test.xml");
			Assert.AreEqual(rss.Tilte(), "Test");
			Assert.AreEqual(rss.Content(), "C'est un test");
			Assert.AreEqual(rss.Author(), "Student DATA RSS");
			Assert.AreEqual(rss.Categorie(), "Etudiant");

			Assert.AreEqual(Helper.TryFileExist(path), true);
			Assert.AreEqual(Helper.TryFileEmpty(path), false);
			//rss.RemoveRSS();

		}


		[TestMethod]
		public void TestAddWithManage()
		{
			RSSManage rssM = new RSSManage();
			List<Flow> ListFlow = new List<Flow>();

			for (int i = 0; i < 4; i++ )
			{
				ListFlow.Add(new Flow("Test"+i, "Le test "+i));
			}
			string path = "testAdd.xml";
			//string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "testAddUpDel.xml");
			RSS rss = rssM.createRSS(path, "TestAddUpDel", "C'est un test d'ajout",Helper.CategorieRSSEnum.Etudiant);
			

			rss.AddFlow(ListFlow);
			
			//A finir
			//rssM.addFlow("testAddUpDel.xml", ListFlow);

			rss.Save(Helper.FormatRSS20());

			Console.WriteLine(rss.GetAllFlow().Count);
			HelpTest.HelpReadWithRSS(rss);

			//Assert.AreEqual(rss.GetAllFlow().Count, 4);
			int i2 = 0;
			foreach (Flow flow in rss.GetAllFlow())
			{
				if ( i2 == 4)
				{
					i2 = 0;
				}
				Assert.AreEqual(flow.Title, "Test" + i2);

				string md5ID = flow.Title + flow.Content + flow.Date;
				string idVar = "";
				using (MD5 md5Hash = MD5.Create())
				{
					idVar = Helper.mD5Hash(md5Hash, md5ID);
				}
				Assert.AreEqual(flow.Id, idVar);
				Assert.AreEqual(flow.Content, "Le test "+i2);
				Assert.AreEqual(flow.Url, "http://www.google.com/");
				i2++;
			}
			Assert.AreEqual(Helper.TryFileExist(path), true);
			Assert.AreEqual(Helper.TryFileEmpty(path), false);
			//rss.RemoveRSS();
		}

		[TestMethod]
		public void testDelWithManage()
		{
			RSSManage rssM = new RSSManage();
			RSS rss = rssM.createRSS("testDel.xml", "TestDel", "C'est un test suppréssion des articles", Helper.CategorieRSSEnum.Etudiant);
			List<Flow> ListFlow = new List<Flow>();

			for (int i = 0; i < 4; i++)
			{
				ListFlow.Add(new Flow("Test" + i, "Le test " + i));
			}

			rss.AddFlow(ListFlow);

			rss.RemoveFlow();

			Assert.AreEqual(rss.GetAllFlow().Count, 0);

			rss.AddFlow(ListFlow);

			Assert.AreEqual(rss.GetAllFlow().Count, 4);

			Flow flowvar = rss.GetAllFlow().Find(x => x.Title == "Test0");
			rss.RemoveFlow(rss.GetAllFlow().Find(x => x.Title == "Test1"));
			rss.RemoveFlow(flowvar.Id);

			rss.Save(Helper.FormatRSS20());

			Console.WriteLine(rss.GetAllFlow().Count);
			HelpTest.HelpReadWithRSS(rss);

			int i2 = 2;
			foreach( Flow flow in rss.GetAllFlow())
			{
				Assert.AreEqual(flow.Title, "Test" + i2);
				string md5ID = flow.Title + flow.Content + flow.Date;
				string idVar = "";
				using (MD5 md5Hash = MD5.Create())
				{
					idVar = Helper.mD5Hash(md5Hash, md5ID);
				}
				Assert.AreEqual(flow.Id, idVar);
				Assert.AreEqual(flow.Content, "Le test " + i2);
				Assert.AreEqual(flow.Url, "http://www.google.com/");
				i2++;
			}

			Flow flowvar2 = rss.GetAllFlow().Find(x => x.Title == "Test3");
			rss.RemoveFlow(flowvar2.Id);

			foreach ( Flow flow in rss.GetAllFlow())
			{
				Assert.AreEqual(flow.Title, "Test2");
				string md5ID = flow.Title + flow.Content + flow.Date;
				string idVar = "";
				using (MD5 md5Hash = MD5.Create())
				{
					idVar = Helper.mD5Hash(md5Hash, md5ID);
				}
				Assert.AreEqual(flow.Id, idVar);
				Assert.AreEqual(flow.Content, "Le test 2");
				Assert.AreEqual(flow.Url, "http://www.google.com/");
			}

			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestUpdateWithMange()
		{
			RSSManage rssM = new RSSManage();
			RSS rss = rssM.createRSS("testUp.xml", "TestUp", "C'est un test modification des articles", Helper.CategorieRSSEnum.Etudiant);
			List<Flow> ListFlow = new List<Flow>();

			for (int i = 0; i < 4; i++)
			{
				ListFlow.Add(new Flow("Test" + i, "Le test " + i));
			}

			rss.AddFlow(ListFlow);

			Flow flowtest = rss.GetAllFlow().Find(x => x.Title == "Test0");

			rss.UpdateFlow(flowtest.Id,"TestTest","tutit");

			rss.Save(Helper.FormatRSS20());

			Flow flow = rss.GetAllFlow().Find(x => x.Title == "TestTest");
			string md5ID = flow.Title + flow.Content + flow.Date;
			string idVar = "";
			using (MD5 md5Hash = MD5.Create())
			{
				idVar = Helper.mD5Hash(md5Hash, md5ID);
			}

			Assert.AreEqual(flow.Title, "TestTest");
			Assert.AreEqual(flow.Content, "tutit");
			Assert.AreEqual(flow.Id, idVar);

			Console.WriteLine(rss.GetAllFlow().Count);
			HelpTest.HelpReadWithRSS(rss);
		}

		[TestMethod]
		public void TestReadRSS()
		{
			//TODO : Gerer exeption mauvais lien url
			string url = "http://www.developpez.com/index/rss";
			string url2 = "https://fr.news.yahoo.com/rss/world";
			string urlFail = "https://fr.news.yahoo.com/sitemap/";
			string url3 = "test.xml";

			RSS rss = new RSS(url);
			RSS rss2 = new RSS(url2);
			RSS rss3 = new RSS(url3);
			RSS rssFail = new RSS(urlFail);
			rss3.RemoveRSS();

			rss.ReadRSS();
			rss2.ReadRSS();
			rss3.ReadRSS();
			rssFail.ReadRSS();

			

			HelpTest.HelpRead(rss.Feed);
			Console.WriteLine("Other FEED WEB\n\n");
			HelpTest.HelpRead(rss2.Feed);
			Console.WriteLine("Other FEED FILE\n\n\n");
			HelpTest.HelpRead(rss3.Feed);
			Console.WriteLine("Other FEED FAIL\n\n\n");
			HelpTest.HelpRead(rssFail.Feed);
			rss3.RemoveRSS();
		}

		[TestMethod]
		public void TestCreateRSS()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			
			rss.InitRSSSingle();
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();

		}

		[TestMethod]
		public void TestAddFlow()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.AddFlowSingle();
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
			
		}

		[TestMethod]
		public void TestDeleteFlow()
		{
			string uri = "test.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.RemoveFlowSingle(8);
			rss.RemoveFlowSingle(id);
			rss.RemoveFlowSingle(id);
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestUpdateFlow()
		{
			string uri = "test.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.UpdateFlowSingle(8);
			rss.UpdateFlowSingle(id);
			rss.RemoveFlowSingle(id);
			rss.UpdateFlowSingle(id);
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}

		[TestMethod]
		public void TestAllRSS()
		{
			string uri = "testRSS.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.AddFlowSingle();
			rss.RemoveFlowSingle(id);
			rss.UpdateFlowSingle(id);
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}


	}
}
