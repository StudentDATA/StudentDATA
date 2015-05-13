using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD.Tests
{
	static class HelpTest
	{
		public static void HelpRead(SyndicationFeed feed)
		{
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
	}
}
