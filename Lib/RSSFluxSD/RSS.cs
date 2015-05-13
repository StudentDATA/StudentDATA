using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RSSFluxSD
{
	public class RSS
	{
		string _uri;
		ReadRSS rRSS;
		CreateRSS cRSS;
		UpdateRSS uRSS;
		SyndicationFeed feed;

		public string Uri
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public RSS(string uri)
		{
			this.Uri = uri;
			cRSS = new CreateRSS();
			rRSS = new ReadRSS(this.Uri);
			uRSS = new UpdateRSS(this.Uri);
		}

		public void InitRSS()
		{
			//Regarder si le fichier existe deja et pas l'ecraser
			feed = cRSS.CreateInit();
			feed.Items = uRSS.AddFlow(feed);
			AddinXml();
		}

		public void AddFlow()
		{
			feed = rRSS.ReadWithURI();
			if ( feed != null)
			{
				feed.Items = uRSS.AddFlow(feed);
				AddinXml();
			}
		}

		public void RemoveFlow()
		{

		}

		public void UpdateFlow()
		{

		}
		private void AddinXml()
		{
			using (var writer = XmlWriter.Create(this.Uri))
			{
				feed.SaveAsRss20(writer);
				writer.Flush();
				writer.Close();
			}
		}
	}
}
