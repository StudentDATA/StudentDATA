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
			RSSManage rssM = new RSSManage();
			rssM.readRSS("https://fr.news.yahoo.com/rss/world");
			rssM.readRSS("http://www.developpez.com/index/rssd");
			foreach ( RSS rss in rssM.GetAllRSS())
			{
				
				Console.WriteLine("Titre : " + rss.Tilte());
				Console.WriteLine();
				Console.WriteLine("Auteur : " + rss.Author());
				Console.WriteLine();
				Console.WriteLine("Categorie : " + rss.Categorie());
				Console.WriteLine();
				Console.WriteLine("Content : " + rss.Content());
				Console.WriteLine();
				Console.WriteLine("Url : " + rss.Url());
				foreach ( Flow flow in rss.GetAllFlow())
				{
					Console.WriteLine("FLOW");
					Console.WriteLine("Titre : " + flow.Title);
					Console.WriteLine();
					Console.WriteLine("Content : " + flow.Content);
					Console.WriteLine();
					Console.WriteLine("Url : " + flow.Url);
					Console.WriteLine();
					Console.WriteLine("ID : " + flow.Id);
					Console.WriteLine();
					Console.WriteLine("Date : " + flow.Date);
				}
			}

		}

		[TestMethod]
		public void TestCreateAndAddXML()
		{
			//TESTER que le fichier n'existe pas deja dans le dossier et dans le dictionaire
			RSSManage rssM = new RSSManage();
			rssM.createRSS("test.xml");
			rssM.createRSS("https://fr.news.yahoo.com/rss/world");
			if ( rssM.Msg_error != null)
			{
				Console.WriteLine(rssM.Msg_error);
			}
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
