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
	public class RSS
	{
		string _uri;
		ReadRSS rRSS;
		CreateRSS cRSS;
		UpdateRSS uRSS;
		SyndicationFeed feed;

		public string Url
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public RSS(string uri)
		{
			this.Url = uri;
			cRSS = new CreateRSS();
			rRSS = new ReadRSS(this.Url);
			uRSS = new UpdateRSS();
		}

		public SyndicationFeed ReadOrCreateRSS()
		{
			//Verifier si c'est un lien ou un fichier
			
			Uri uriResult;
			bool result = Uri.TryCreate(Url, UriKind.Absolute, out uriResult)
				&& (   uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps );

			if (File.Exists(Url) || result)
			{
				return rRSS.ReadWithURI();
			}
			else
			{
				feed = cRSS.CreateInit();
				AddinXml();
				return feed;
			}

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
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.AddFlow(feed);
			AddinXml();
		}

		public void RemoveFlow(int id)
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.DeleteFlow(id, feed);
			AddinXml();
		}

		public void UpdateFlow(int id)
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.UpdateFlow(id,feed);
			AddinXml();
		}
		private void AddinXml()
		{
			using (var writer = XmlWriter.Create(this.Url))
			{
				feed.SaveAsRss20(writer);
				writer.Flush();
				writer.Close();
			}
		}
	}
}
