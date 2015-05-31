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

		string _uri_rss;
		string _msg_error;
		bool _feedIsNull;

		ReadRSS rRSS;
		CreateRSS cRSS;
		UpdateRSS uRSS;
		SyndicationFeed _feed;
		Helper.FormatRSSEnum formatSS;

		public Helper.FormatRSSEnum FormatSS
		{
			get { return formatSS; }
			set { formatSS = value; }
		}

		public SyndicationFeed Feed
		{
			get { return _feed; }
			private set { _feed = value; }
		}
		public bool FeedIsNull
		{
			get { return _feedIsNull; }
			private set { _feedIsNull = value; }
		}

		List<Flow> flowList;

		public List<Flow> GetAllFlow()
		{
			return flowList;
		}

		public string Tilte()
		{
			return cRSS.Titre;
		}

		public string Author()
		{
			return cRSS.Author;
		}

		public string Categorie()
		{
			return cRSS.Categorie;
		}

		public string Url()
		{
			return cRSS.Url;
		}

		public string Content()
		{
			return cRSS.Content;
		}

		public string Uri_RSS
		{
			get { return _uri_rss; }
			set { _uri_rss = value; }
		}

		public RSS(string url, Helper.FormatRSSEnum formatRss)
		{
			this.Uri_RSS = url;
			this.FormatSS = formatRss;
			flowList = new List<Flow>();
			rRSS = new ReadRSS(this.Uri_RSS);
			cRSS = new CreateRSS();
			uRSS = new UpdateRSS();

		}

		public void ReadRSS()
		{
			
			//Verifie si ce n'est pas un lien et si le fichier existe.
			if (File.Exists(Uri_RSS) || Helper.TryUri(Uri_RSS))
			{
				string linkV = "";
				string linkFeed = "";
				string titleFeed = "";
				string authorFeed = "";
				string categorieFeed = "";
				string descriptionFeed = "";
				Feed = rRSS.ReadWithURI();

				if ( Feed != null )
				{
					FeedIsNull = false;
					if (Feed.Title != null)
					{
						titleFeed = Feed.Title.Text;
					}

					if (Feed.Links.Count > 0)
					{
						linkFeed = Feed.Links.LastOrDefault().Uri.AbsoluteUri;
					}

					if (Feed.Authors.Count > 0)
					{
						if ( Helper.TryUri(Uri_RSS))
						{
							foreach (SyndicationPerson author in Feed.Authors)
							{
								authorFeed += author.Name + " ";
							}
						}
						else
						{
							foreach ( SyndicationPerson author in Feed.Authors)
							{
								authorFeed += author.Email + " ";
							}
						}

					}

					if (Feed.Categories.Count > 0)
					{
						foreach ( SyndicationCategory categorie in Feed.Categories)
						{
							categorieFeed += categorie.Name+" ";
						}	
					}

					if (Feed.Description.Text != null)
					{
						descriptionFeed = Feed.Description.Text;
					}

					cRSS.AddInitRSS(Feed.Title.Text, linkFeed, authorFeed, categorieFeed, descriptionFeed);

					foreach (SyndicationItem item in Feed.Items)
					{
						foreach (SyndicationLink link in item.Links)
						{
							if (link.MediaType != "image/jpeg")
							{
								linkV = link.Uri.OriginalString;
							}
						}
						//Verifie l'id du post pour savoir evité les doublons mais à coriger.
						//if (flowList.Find(x => x.Id == item.Id) == null)
						//{
							flowList.Add(new Flow(item.Title.Text, ((TextSyndicationContent)item.Summary).Text, linkV, item.Id, item.LastUpdatedTime));
						//}
					}
				}
				else
				{
					_msg_error = "Aucune donnée exploitable";
					FeedIsNull = true;
				}
			}
			else
			{
				//Demande de création du rss
				//Feed = cRSS.CreateInit();
			}
		}

		public void InitRSSSingle()
		{
			cRSS.InitRSSBase();
			Feed = cRSS.CreateInit();
			Save(Helper.FormatRSS20());
		}

		public void InitRSS(string title, Helper.CategorieRSSEnum categorie, string content)
		{

			cRSS.CreateRSSInit(title,categorie,content);
			Feed = cRSS.CreateInit();
		}

		public void AddFlowSingle()
		{
			ReadRSS();
			Feed.Items = uRSS.AddFlow(Feed);
			Save(Helper.FormatRSS20());
		}

		public void AddFlow()
		{
			if (!Helper.TryUri(Uri_RSS))
			{
				flowList.Add(new Flow("1", "2", "3", "4", DateTimeOffset.Now));
				Feed.Items = uRSS.AddFlow(flowList);
			}
		}

		public void AddFlow(List<Flow> ListFlow)
		{
			if (!Helper.TryUri(Uri_RSS))
			{
				flowList.AddRange(ListFlow);
				Feed.Items = uRSS.AddFlow(flowList);
			}
		}

		public void RemoveFlowSingle(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.DeleteFlow(id, Feed);
			Save(Helper.FormatRSS20());
		}

		public void RemoveFlow()
		{
			flowList.Clear();
			Feed.Items = new List < SyndicationItem >();
		}

		public void RemoveFlow(Flow flow)
		{
			flowList.Remove(flow);
			Feed.Items = uRSS.DeleteFlow(flow,Feed);
		}

		public void RemoveFlow(string title)
		{
			Flow flow = flowList.Find(x => x.Title == title);
			RemoveFlow(flow);
		}

		public void UpdateFlow(Flow flow)
		{
			Feed.Items = uRSS.UpdateFlow(flow, Feed);
		}

		public void UpdateFlow(string title)
		{
			Flow flow = GetAllFlow().Find(x => x.Title == title);
			Feed.Items = uRSS.UpdateFlow(flow, Feed);
		}

		public void UpdateFlowSingle(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.UpdateFlow(id,Feed);
			Save(Helper.FormatRSS20());
		}
		public void Save(Helper.FormatRSSEnum format )
		{
			//Savoir si le fichier existe deja ou pas
			if ( Feed != null )
			{
				using (var writer = XmlWriter.Create(this.Uri_RSS))
				{
					if ( format == Helper.FormatRSSEnum.RSS20)
					{
						Feed.SaveAsRss20(writer);
					}
					else if ( format == Helper.FormatRSSEnum.Atom10)
					{
						Feed.SaveAsAtom10(writer);
					}
					writer.Flush();
					writer.Close();
				}
			}

		}

		public void RemoveRSS()
		{
			if (File.Exists(Uri_RSS))
			{
				File.Delete(Uri_RSS);
			}
		}
	}
}
