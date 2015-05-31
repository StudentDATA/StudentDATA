using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSSFluxSD;
using System.Threading;
using System.ServiceModel.Syndication;
using System.Xml;
using System.Collections.Generic;

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
			
			RSS rss1 = rssM.createRSS("test.xml","Test","C'est un test",Helper.CategorieRSSEnum.Etudiant);
			RSS rss = rssM.createRSS("https://fr.news.yahoo.com/rss/world","Test","C'est un test",Helper.CategorieRSSEnum.Etudiant);

			if ( rssM.Msg_error != null)
			{
				Console.WriteLine(rssM.Msg_error);
			}
			rss1.Save();
			HelpTest.HelpReadWithManage(rssM);
		}


		[TestMethod]
		public void TestAddUpDel()
		{
			RSSManage rssM = new RSSManage();
			List<Flow> ListFlow = new List<Flow>();

			for (int i = 0; i < 4; i++ )
			{
				ListFlow.Add(new Flow("Test"+i, "Le test "+i));
			}

			RSS rss = rssM.createRSS("testAddUpDel.xml", "TestAddUpDel", "C'est un test d'ajout, supp et modif", Helper.CategorieRSSEnum.Etudiant);
			
			rss.AddFlow(ListFlow);
			//rssM.addFlow("testAddUpDel.xml", ListFlow);

			//Remove : Methode 1(Sup all)
			//rss.RemoveFlow();

			//Remove : Methode 2(Supp 1 Flow)
			rss.RemoveFlow(rss.GetAllFlow().Find(x => x.Title == "Test2"));

			//Remove : Methode 3(Supp 1 flow par titre)
			rss.RemoveFlow("Test2");

			//Update par flow
			//rss.UpdateFlow(flow);

			//Update par titre

			rss.Save();

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
