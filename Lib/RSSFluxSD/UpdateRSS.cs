using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RSSFluxSD
{
	public class UpdateRSS
	{

		List<SyndicationItem> items;


		public List<SyndicationItem> AddArticle(SyndicationFeed feed)
		{
			SyndicationItem item = new SyndicationItem(
				"Item One5",
				"This is the content for item one4",
				new Uri("http://localhost/Content/One5"),
				"ItemOneID5",
				DateTime.Now);

			List<SyndicationItem> items = GetItemFeed(feed);

			items.Add(item);

			return items;
		}

		public List<SyndicationItem> AddArticle(List<Article> articleList)
		{
			List<SyndicationItem> items = new List<SyndicationItem>();
			foreach ( Article article in articleList)
			{
				items.Add(new SyndicationItem(
					article.Title,
					article.Content,
					new Uri(article.Url),
					article.Id,
					article.Date));
			}

			//List<SyndicationItem> itemsList = GetItemFeed(feed);
			//itemsList.AddRange(items);			

			//List<SyndicationItem> itemsList = items;


			return items;
		}


		public List<SyndicationItem> AddArticle(Article article)
		{
			List<SyndicationItem> items = new List<SyndicationItem>();
			items.Add(new SyndicationItem(
				article.Title,
				article.Content,
				new Uri(article.Url),
				article.Id,
				article.Date));

			//List<SyndicationItem> itemsList = GetItemFeed(feed);
			//itemsList.AddRange(items);			

			//List<SyndicationItem> itemsList = items;


			return items;
		}


		public List<SyndicationItem> UpdateArticle(int id, SyndicationFeed feed)
		{
			string msgError = "L'article n'existe pas";
			List<SyndicationItem> items = GetItemFeed(feed);
			if (items.Count > 0)
			{
				try
				{	
					items = DeleteArticle(id, feed);
					feed.Items = items;
					items.InsertRange(id, AddArticle(new Article ("Titre", "Content", "http://url.com", "IdArticle1",DateTimeOffset.Now)));				
					return items;
				}
				catch
				{
					Console.WriteLine(msgError);
					return items;
				}
			}
			else
			{
				Console.WriteLine(msgError);
				return items;
			}
		}


		public List<SyndicationItem> DeleteArticle(int id, SyndicationFeed feed)
		{
			List<SyndicationItem> items = GetItemFeed(feed);
			string msgError = "L'article n'existe pas";

			if ( items.Count > 0)
			{
				try
				{
					items.RemoveAt(id);
					return items;
				}
				catch
				{
					Console.WriteLine(msgError);
					return items;
				}
			}
			else
			{
				Console.WriteLine(msgError);
				return items;
			}
		}

		public List<SyndicationItem> DeleteArticle(Article article,SyndicationFeed feed)
		{
			List<SyndicationItem> items = GetItemFeed(feed);
			string msgError = "L'article n'existe pas";

			if ( feed.Items.Count() > 0)
			{
				try
				{
					SyndicationItem item = items.Find(x => x.Title.Text == article.Title && x.Id == article.Id);
					items.Remove(item);
					return items;
				}
				catch
				{
					Console.WriteLine(msgError);
					return items;
				}
			}
			else
			{
				Console.WriteLine(msgError);
				return items;
			}
		}


		public List<SyndicationItem> GetItemFeed(SyndicationFeed feed)
		{
			items = new List<SyndicationItem>();

			if ( feed.Items.Count() > 0 )
			{
				foreach (SyndicationItem itemFeed in feed.Items)
				{
					items.Add(itemFeed);
				}
			}
			return items;
		}

	}
}
