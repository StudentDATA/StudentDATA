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
		string _msg_error;

		ReadRSS rRSS;
		CreateRSS cRSS;
		UpdateRSS uRSS;
		SyndicationFeed feed;
		List<Flow> flow;
		IReadOnlyList<Flow> FlowRead;

		public IReadOnlyList<Flow> GetAllFlow()
		{
			return FlowRead = flow;
		}

		public CreateRSS GetRSS()
		{
			return cRSS;
		}

		public string Url
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public RSS(string uri)
		{
			this.Url = uri;
			flow = new List<Flow>();
			cRSS = new CreateRSS();
			rRSS = new ReadRSS(this.Url);
			uRSS = new UpdateRSS();
			
		}

		public SyndicationFeed ReadOrCreateRSS()
		{
			//TODO : Verifier si c'est un lien ou un fichier
			
			Uri uriResult;
			bool result = Uri.TryCreate(Url, UriKind.Absolute, out uriResult)
				&& ( uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps );

			if (File.Exists(Url) || result)
			{
				return rRSS.ReadWithURI();
			}
			else
			{
				//Demande de création du rss
				feed = cRSS.CreateInit();
				return feed;
			}

		}

		public void InitRSSSingle()
		{
			InitRSS();
			AddinXml();
		}

		public void InitRSS()
		{
			feed = cRSS.CreateInit();
			
		}

		//TODO : Pouvoir en ajouter plusieurs sans ecrire dans le xml 1 par 1
		public void AddFlowSingle()
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.AddFlow(feed);
			AddinXml();
		}

		public void AddFlow()
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.AddFlow(feed);
		}


		//Pouvoir en retirer plusieurs sans ecrire dans le xml 1 par 1
		public void RemoveFlowSingle(int id)
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.DeleteFlow(id, feed);
			AddinXml();
		}

		public void RemoveFlow(int id)
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.DeleteFlow(id, feed);
			AddinXml();
		}

		//TODO : Pouvoir en modifier plusieurs sans ecrire dans le xml 1 par 1
		public void UpdateFlowSingle(int id)
		{
			feed = ReadOrCreateRSS();
			feed.Items = uRSS.UpdateFlow(id,feed);
			AddinXml();
		}
		private void AddinXml()
		{
			//Savoir si le fichier existe deja ou pas
			using (var writer = XmlWriter.Create(this.Url))
			{
				feed.SaveAsRss20(writer);
				writer.Flush();
				writer.Close();
			}
		}

		public void RemoveRSS()
		{
			if (File.Exists(Url))
			{
				File.Delete(Url);
			}
		}
	}
}
