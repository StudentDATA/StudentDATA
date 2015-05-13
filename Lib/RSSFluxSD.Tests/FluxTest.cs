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
			ReadRSS flux = new ReadRSS("test.xml");

			//SyndicationFeed feed = flux.ReadWithURI("http://www.developpez.com/index/rss");
			SyndicationFeed feed = flux.ReadWithURI();

			Console.WriteLine(feed.Title.Text);
			Console.WriteLine("Items:");
			foreach (SyndicationItem item in feed.Items)
			{
				Console.WriteLine("Title: {0}\n", item.Title.Text);
				Console.WriteLine("Summary: {0}\n", ((TextSyndicationContent)item.Summary).Text);
				foreach (SyndicationLink link in item.Links)
				{

					if (link.MediaType != "image/jpeg")
					{
						Console.WriteLine("Test: {0}", link.MediaType);
						Console.WriteLine("Link: {0}\n", link.Uri.AbsoluteUri);
					}

				}
				Console.WriteLine("Date: {0}\n", item.PublishDate.DateTime);
			}

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
			UpdateRSS aFlow = new UpdateRSS(uri);
			//aFlow.DeleteFlow(0);

		}


	}
}
