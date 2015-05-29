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
			//TESTER que le fichier n'existe pas deja dans le dossier et dans le dictionaire
			List<string> testFlow = new List<string> { "Jaune", "C'est jaune", "Vert", "C'est Vert"};
			RSSManage rssM = new RSSManage();
			List<Flow> ListFlow = new List<Flow>();


			Flow flow = new Flow("1", "2");



			rssM.createRSS("test.xml");
			rssM.createRSS("https://fr.news.yahoo.com/rss/world");
			//rssM.addFlow("test.xml", testFlow);
			rssM.addFlow("test.xml", ListFlow);
			if ( rssM.Msg_error != null)
			{
				Console.WriteLine(rssM.Msg_error);
			}

			HelpTest.HelpReadWithManage(rssM);
		}

		public void TestAddFlowWithManager()
		{

		}
		//addflow Supp, Update File par le manager.
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
