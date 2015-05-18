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
		public void TestL()
		{
			RSSManage rssM = new RSSManage();
			foreach (RSS rss in rssM.GetAllRSS())
			{
				//rss.GetRSS().Titre = "1";
				Console.WriteLine(rss.GetRSS().Titre);
				Console.WriteLine();
				Console.WriteLine(rss.GetRSS().TitreDescription);
				Console.WriteLine();
				Console.WriteLine(rss.GetRSS().Author);
				Console.WriteLine();
				Console.WriteLine(rss.GetRSS().Categorie);
				Console.WriteLine();
				Console.WriteLine(rss.GetRSS().Content);
				Console.WriteLine();
				Console.WriteLine(rss.GetRSS().Url);
			}

		}
		[TestMethod]
		public void TestReadRSS()
		{
			//TODO : Gerer exeption mauvais lien url
			string url = "http://www.developpez.com/index/rss/ff";
			string url2 = "https://fr.news.yahoo.com/rss/world";
			string urlFail = "https://fr.news.yahoo.com/sitemap/";
			string url3 = "test.xml";

			RSS rss = new RSS(url);
			RSS rss2 = new RSS(url2);
			RSS rss3 = new RSS(url3);
			RSS rssFail = new RSS(urlFail);
			rss3.RemoveRSS();

			SyndicationFeed feed = rss.ReadOrCreateRSS();
			SyndicationFeed feed2 = rss2.ReadOrCreateRSS();
			SyndicationFeed feed3 = rss3.ReadOrCreateRSS();
			SyndicationFeed feedFail = rssFail.ReadOrCreateRSS();

			HelpTest.HelpRead(feed);
			Console.WriteLine("Other FEED WEB\n\n");
			HelpTest.HelpRead(feed2);
			Console.WriteLine("Other FEED FILE\n\n\n");
			HelpTest.HelpRead(feed3);
			Console.WriteLine("Other FEED FAIL\n\n\n");
			HelpTest.HelpRead(feedFail);
			rss3.RemoveRSS();
		}

		[TestMethod]
		public void TestCreateRSS()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			
			rss.InitRSSSingle();
			SyndicationFeed feed = rss.ReadOrCreateRSS();
			HelpTest.HelpRead(feed);
			rss.RemoveRSS();

		}

		[TestMethod]
		public void TestAddFlow()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.AddFlowSingle();
			SyndicationFeed feed = rss.ReadOrCreateRSS();
			HelpTest.HelpRead(feed);
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
			SyndicationFeed feed = rss.ReadOrCreateRSS();
			HelpTest.HelpRead(feed);
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
			SyndicationFeed feed = rss.ReadOrCreateRSS();
			HelpTest.HelpRead(feed);
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
			SyndicationFeed feed = rss.ReadOrCreateRSS();
			HelpTest.HelpRead(feed);
			rss.RemoveRSS();
		}


	}
}
