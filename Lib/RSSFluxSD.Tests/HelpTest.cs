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
			foreach (Article article in rss.GetAllArticle())
			{
				Console.WriteLine("ARTICLE");
				Console.WriteLine("Titre : " + article.Title);
				Console.WriteLine();
				Console.WriteLine("Content : " + article.Content);
				Console.WriteLine();
				Console.WriteLine("Url : " + article.Url);
				Console.WriteLine();
				Console.WriteLine("ID : " + article.Id);
				Console.WriteLine();
				Console.WriteLine("Date : " + article.Date);
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
				foreach (Article article in rss.GetAllArticle())
				{
					Console.WriteLine("ARTICLE");
					Console.WriteLine("Titre : " + article.Title);
					Console.WriteLine();
					Console.WriteLine("Content : " + article.Content);
					Console.WriteLine();
					Console.WriteLine("Url : " + article.Url);
					Console.WriteLine();
					Console.WriteLine("ID : " + article.Id);
					Console.WriteLine();
					Console.WriteLine("Date : " + article.Date);
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
