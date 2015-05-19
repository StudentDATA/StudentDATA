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
			rssM.addRSS("https://fr.news.yahoo.com/rss/world");
			foreach (RSS rss in rssM.GetAllRSS())
			{
				Console.WriteLine(rss.GetStartRSS().Titre);
				Console.WriteLine();
				Console.WriteLine(rss.GetStartRSS().Author);
				Console.WriteLine();
				Console.WriteLine(rss.GetStartRSS().Categorie);
				Console.WriteLine();
				Console.WriteLine(rss.GetStartRSS().Content);
				Console.WriteLine();
				Console.WriteLine(rss.GetStartRSS().Url);
				foreach ( Flow flow in rss.GetAllFlow())
				{
					Console.WriteLine("FLOW");
					Console.WriteLine(flow.Title);
					Console.WriteLine();
					Console.WriteLine(flow.Content);
					Console.WriteLine();
					Console.WriteLine(flow.Url);
					Console.WriteLine();
					Console.WriteLine(flow.Id);
					Console.WriteLine();
				}
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

			rss.ReadOrCreateRSS();
			rss2.ReadOrCreateRSS();
			rss3.ReadOrCreateRSS();
			rssFail.ReadOrCreateRSS();

			

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
			rss.ReadOrCreateRSS();
			HelpTest.HelpRead(rss.Feed);
			//rss.RemoveRSS();

		}

		[TestMethod]
		public void TestAddFlow()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			rss.InitRSSSingle();
			rss.AddFlowSingle();
			rss.ReadOrCreateRSS();
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
			rss.ReadOrCreateRSS();
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
			rss.ReadOrCreateRSS();
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
			rss.ReadOrCreateRSS();
			HelpTest.HelpRead(rss.Feed);
			rss.RemoveRSS();
		}


	}
}
