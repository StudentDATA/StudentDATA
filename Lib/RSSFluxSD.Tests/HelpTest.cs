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
		public static void HelpReadWithRSS(RSS rss)
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
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			foreach (Flow flow in rss.GetAllFlow())
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

		public static void HelpReadWithManage(RSSManage rssM)
		{
			foreach (RSS rss in rssM.GetAllRSS())
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
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine();
				Console.WriteLine();
				foreach (Flow flow in rss.GetAllFlow())
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
		public static void HelpRead(SyndicationFeed feed)
		{
			if ( feed != null)
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
}
