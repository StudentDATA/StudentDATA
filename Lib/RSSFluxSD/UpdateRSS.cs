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
		string _uri;
		List<SyndicationItem> items;

		public string Uri
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public UpdateRSS(string uri)
		{
			this.Uri = uri;
		}



		public List<SyndicationItem> AddFlow(SyndicationFeed feed)
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

		public void UpdateFlow()
		{

			List<SyndicationItem> items = new List<SyndicationItem>();


		}
		public void DeleteFlow(int id, SyndicationFeed feed)
		{
			List<SyndicationItem> items = GetItemFeed(feed);


			if ( items.Count > 0)
			{
				try
				{
					items.RemoveAt(id);
				}
				catch
				{
					Console.WriteLine("L'article n'existe pas");
				}
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
