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
		SyndicationFeed _feed;

		public SyndicationFeed Feed
		{
			get { return _feed; }
			private set { _feed = value; }
		}

		List<Flow> flowList;

		public IReadOnlyList<Flow> GetAllFlow()
		{
			return flowList;
		}

		public CreateRSS GetHeaderRSS()
		{
			return cRSS;
		}

		public string Url
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public RSS(string url)
		{
			this.Url = url;
			flowList = new List<Flow>();
			rRSS = new ReadRSS(this.Url);
			cRSS = new CreateRSS();
			uRSS = new UpdateRSS();
			
		}

		public void ReadRSS()
		{
			//TODO : Verifier si c'est un lien ou un fichier
			
			Uri uriResult;
			bool result = Uri.TryCreate(Url, UriKind.Absolute, out uriResult)
				&& ( uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps );

			if (File.Exists(Url) || result)
			{
				string linkV;
				string linkFeed;
				string titleFeed;
				string authorFeed;
				string categorieFeed;
				string descriptionFeed;
				Feed = rRSS.ReadWithURI();

				if ( Feed != null )
				{
					if (Feed.Title != null)
					{
						titleFeed = Feed.Title.Text;
					}
					else
					{
						titleFeed = "";
					}

					if (Feed.Links.Count > 0)
					{
						linkFeed = Feed.Links.LastOrDefault().Uri.AbsoluteUri;
					}
					else
					{
						linkFeed = "";
					}

					if (Feed.Authors.Count > 0)
					{
						authorFeed = Feed.Authors.LastOrDefault().ToString();
					}
					else
					{
						authorFeed = "";
					}

					if (Feed.Categories.Count > 0)
					{
						categorieFeed = Feed.Categories.LastOrDefault().Name;
					}
					else
					{
						categorieFeed = "";
					}

					if (Feed.Description.Text != null)
					{
						descriptionFeed = Feed.Description.Text;
					}
					else
					{
						descriptionFeed = "";
					}
					cRSS.AddInitRSS(Feed.Title.Text, linkFeed, authorFeed, categorieFeed, descriptionFeed);

					linkV = "";
					foreach (SyndicationItem item in Feed.Items)
					{
						foreach (SyndicationLink link in item.Links)
						{
							linkV = "";
							if (link.MediaType != "image/jpeg")
							{
								linkV = link.Uri.OriginalString;
							}
						}
						flowList.Add(new Flow(item.Title.Text, ((TextSyndicationContent)item.Summary).Text, linkV, item.Id));
					}
				}
			}
			else
			{
				//Demande de création du rss
				Feed = cRSS.CreateInit();
			}
		}

		public void InitRSSSingle()
		{
			InitRSS();
			AddinXml();
		}

		public void InitRSS()
		{
			Feed = cRSS.CreateInit();
		}

		//TODO : Pouvoir en ajouter plusieurs sans ecrire dans le xml 1 par 1
		public void AddFlowSingle()
		{
			ReadRSS();
			Feed.Items = uRSS.AddFlow(Feed);
			AddinXml();
		}

		public void AddFlow()
		{
			ReadRSS();
			flowList.Add(new Flow("1", "2", "3", "4"));
			Feed.Items = uRSS.AddFlow(Feed,flowList);
		}


		//Pouvoir en retirer plusieurs sans ecrire dans le xml 1 par 1
		public void RemoveFlowSingle(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.DeleteFlow(id, Feed);
			AddinXml();
		}

		public void RemoveFlow(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.DeleteFlow(id, Feed);
			AddinXml();
		}

		//TODO : Pouvoir en modifier plusieurs sans ecrire dans le xml 1 par 1
		public void UpdateFlowSingle(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.UpdateFlow(id,Feed);
			AddinXml();
		}
		public void AddinXml()
		{
			//Savoir si le fichier existe deja ou pas
			if ( Feed != null )
			{
				using (var writer = XmlWriter.Create(this.Url))
				{
					Feed.SaveAsRss20(writer);
					writer.Flush();
					writer.Close();
				}
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
