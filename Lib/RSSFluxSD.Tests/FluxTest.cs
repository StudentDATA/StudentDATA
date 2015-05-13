using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSSFluxSD;
using System.Threading;
using System.ServiceModel.Syndication;
using System.Xml;

namespace RSSFluxSD.Tests
{
	[TestClass]
	public class FluxTest
	{
		[TestMethod]
		public void TestReadRSS()
		{
			string uri = "http://www.developpez.com/index/rss";
			RSS rss = new RSS(uri);

			SyndicationFeed feed = rss.ReadOrCreateRSS();

			//SyndicationFeed feed = flux.ReadWithURI("http://www.developpez.com/index/rss");

			HelpTest.HelpRead(feed);

		}

		[TestMethod]
		public void TestCreateRSS()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);

			rss.InitRSS();
		}

		[TestMethod]
		public void TestAddFlow()
		{
			string uri = "test.xml";
			RSS rss = new RSS(uri);
			rss.AddFlow();
			
		}

		[TestMethod]
		public void TestDeleteFlow()
		{
			string uri = "test.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.RemoveFlow(id);
		}

		[TestMethod]
		public void TestUpdateFlow()
		{
			string uri = "test.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.UpdateFlow(id);
		}

		[TestMethod]
		public void TestAllRSS()
		{
			string uri = "testRSS.xml";
			int id = 0;
			RSS rss = new RSS(uri);
			rss.InitRSS();
			rss.AddFlow();
			rss.RemoveFlow(id);
			rss.UpdateFlow(id);
			SyndicationFeed feed = rss.ReadOrCreateRSS();
			HelpTest.HelpRead(feed);
		}


	}
}
