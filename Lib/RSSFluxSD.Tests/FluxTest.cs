using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSSFluxSD;
using System.Threading;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Collections.Generic;
using System.IO;

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
			RSS rss1 = rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			rssM.readRSS("http://www.developpez.com/index/rss");

			Console.WriteLine(rss1.Tilte());
			foreach ( Flow flow in rss1.GetAllFlow())
			{
				Console.WriteLine(flow.Title);
				Console.WriteLine(flow.Content);
			}

			Console.WriteLine(rssM.GetAllRSS().Count);
			foreach( RSS rss in rssM.GetAllRSS())
			{
				Console.WriteLine(rss.GetAllFlow().Count);
			}
			Console.WriteLine();

			HelpTest.HelpReadWithManage(rssM);

		}

		[TestMethod]
		public void TestCreateAndAddXML()
		{
			RSSManage rssM = new RSSManage();
 
			RSS rss1 = rssM.createRSS("test.xml", "Test", "C'est un test", Helper.CategorieRSSEnum.Etudiant);
			RSS rss = rssM.createRSS("https://fr.news.yahoo.com/rss/world", "Test", "C'est un test", Helper.CategorieRSSEnum.Etudiant);

			if ( rssM.Msg_error != null)
			{
				Console.WriteLine(rssM.Msg_error);
			}
			rss1.Save(Helper.FormatRSSEnum.RSS20);
			HelpTest.HelpReadWithManage(rssM);

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

			//string path = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "testAddUpDel.xml");
			RSS rss = rssM.createRSS("testAdd.xml", "TestAddUpDel", "C'est un test d'ajout",Helper.CategorieRSSEnum.Etudiant);
			

			rss.AddFlow(ListFlow);
			
			//A finir
			//rssM.addFlow("testAddUpDel.xml", ListFlow);

			rss.Save(Helper.FormatRSS20());

			Console.WriteLine(rss.GetAllFlow().Count);
			HelpTest.HelpReadWithRSS(rss);
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

			rss.AddFlow(ListFlow);

			rss.RemoveFlow(rss.GetAllFlow().Find(x => x.Title == "Test1"));
			rss.RemoveFlow("Tilte0");

			rss.Save(Helper.FormatRSS20());

			Console.WriteLine(rss.GetAllFlow().Count);
			HelpTest.HelpReadWithRSS(rss);
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
			
			//Aaméliorer : Bug de déboublement de post
			//rss.UpdateFlow(rss.GetAllFlow().Find(x => x.Title == "Test2"));
			//Aaméliorer
			//rss.UpdateFlow("Test1");

			rss.Save(Helper.FormatRSS20());

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

			RSS rss = new RSS(url,Helper.FormatRSS20());
			RSS rss2 = new RSS(url2, Helper.FormatRSS20());
			RSS rss3 = new RSS(url3, Helper.FormatRSS20());
			RSS rssFail = new RSS(urlFail, Helper.FormatRSS20());
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
			RSS rss = new RSS(uri, Helper.FormatRSS20());
			
			rss.InitRSSSingle();
			rss.ReadRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();

		}

		[TestMethod]
		public void TestAddFlow()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri, Helper.FormatRSS20());
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
			RSS rss = new RSS(uri, Helper.FormatRSS20());
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
			RSS rss = new RSS(uri, Helper.FormatRSS20());
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
			RSS rss = new RSS(uri, Helper.FormatRSS20());
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
