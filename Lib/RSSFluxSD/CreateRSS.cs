using System;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text;

namespace RSSFluxSD
{

		public class CreateRSS
		{

			public SyndicationFeed CreateInit()
			{
				SyndicationFeed feed = new SyndicationFeed("My Blog Feed", "This is a test feed", new Uri("http://SomeURI"));
				feed.Authors.Add(new SyndicationPerson("someone@microsoft.com"));
				feed.Categories.Add(new SyndicationCategory("How To Sample Code"));
				feed.Description = new TextSyndicationContent("This is a how to sample that demonstrates how to expose a feed using RSS with WCF");
				return feed;
			}

		}



}
