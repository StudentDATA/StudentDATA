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
		bool _isUri;

		ReadRSS rRSS;
		CreateRSS cRSS;
		UpdateRSS uRSS;
		SyndicationFeed _feed;


		public SyndicationFeed Feed
		{
			get { return _feed; }
			private set { _feed = value; }
		}

		public bool IsUri
		{
			get { return _isUri; }
			private set { _isUri = value; }
		}

		public bool FeedIsNull
		{
			get { return _feedIsNull; }
			private set { _feedIsNull = value; }
		}

		List<Article> articleList;

		public List<Article> GetAllArticle()
		{
			return articleList;
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

		public RSS(string url)
		{
			this.Uri_RSS = url;
			this.IsUri = Helper.TryUri(url);
			articleList = new List<Article>();
			rRSS = new ReadRSS(this.Uri_RSS);
			cRSS = new CreateRSS();
			uRSS = new UpdateRSS();
		}

		public RSS(bool feedisnull)
		{
			this.Uri_RSS = "";
			FeedIsNull = feedisnull;
			articleList = new List<Article>();
			rRSS = new ReadRSS(this.Uri_RSS);
			cRSS = new CreateRSS();
			uRSS = new UpdateRSS();
		}

		public void ReadRSS()
		{
			
			if (File.Exists(Uri_RSS) || IsUri)
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
						if (IsUri)
						{
							foreach (SyndicationPerson author in Feed.Authors)
							{
								authorFeed += author.Name + " ";
							}
						}
						else
						{
							//A améliorer
							foreach ( SyndicationPerson author in Feed.Authors)
							{
								authorFeed += author.Email;
							}
						}

					}

					if (Feed.Categories.Count > 0)
					{
						//A améliorer
						foreach ( SyndicationCategory categorie in Feed.Categories)
						{
							categorieFeed += categorie.Name;
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
						//if (articleList.Find(x => x.Id == item.Id) == null)
						//{
							articleList.Add(new Article(item.Title.Text, ((TextSyndicationContent)item.Summary).Text, linkV, item.Id, item.LastUpdatedTime));
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
				FeedIsNull = true;
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

		public void AddArticleSingle()
		{
			ReadRSS();
			Feed.Items = uRSS.AddArticle(Feed);
			Save(Helper.FormatRSS20());
		}

		public void AddArticle()
		{
			if (!IsUri && !FeedIsNull)
			{
				articleList.Add(new Article("1", "2", "3", "4", DateTimeOffset.Now));
				Feed.Items = uRSS.AddArticle(articleList);
			}
		}

		public void AddArticle(List<Article> ListArticle)
		{
			if (!IsUri && !FeedIsNull)
			{
				articleList.AddRange(ListArticle);
				Feed.Items = uRSS.AddArticle(articleList);
			}
		}

		public void RemoveArticleSingle(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.DeleteArticle(id, Feed);
			Save(Helper.FormatRSS20());
		}

		public void RemoveArticle()
		{
			if (!IsUri && !FeedIsNull)
			{
				articleList.Clear();
				Feed.Items = new List < SyndicationItem >();
			}
		}

		public void RemoveArticle(Article article)
		{
			if (!IsUri && !FeedIsNull)
			{
				articleList.Remove(article);
				Feed.Items = uRSS.DeleteArticle(article, Feed);
			}
		}

		public void RemoveArticle(string id)
		{
			if (!IsUri && !FeedIsNull)
			{
				Article article = articleList.Find(x => x.Id == id);
				RemoveArticle(article);
			}
		}


		public void UpdateArticle(string id,string title,string content)
		{
			if (!IsUri && !FeedIsNull)
			{
				//Mettre les proprieté en internal et changer les donnée à l'intérieur BY TOTO
				Predicate<Article> preArticle = x => x.Id == id;
				//int idList = articleList.FindIndex(preArticle);
				Article articlevar = articleList.Find(preArticle);
				articlevar.Content = content;
				articlevar.Title = title;
				articlevar.updateID();
				//RemoveArticle(articlevar);
				//articleList.Insert(idList, new Article(title, content));
				Feed.Items = uRSS.AddArticle(articleList);
			}
		}

		public void UpdateArticle(string id, string text, bool title)
		{
			if (!IsUri && !FeedIsNull)
			{
				Predicate<Article> preArticle = x => x.Id == id;
				int idList = articleList.FindIndex(preArticle);
				Article articlevar = articleList.Find(preArticle);
				RemoveArticle(articlevar);
				if( title)
				{
					articleList.Insert(idList, new Article(text, articlevar.Content));
				}
				else
				{
					articleList.Insert(idList, new Article(articlevar.Title, text));
				}
				Feed.Items = uRSS.AddArticle(articleList);
			}
		}

		public void UpdateArticleSingle(int id)
		{
			ReadRSS();
			Feed.Items = uRSS.UpdateArticle(id,Feed);
			Save(Helper.FormatRSS20());
		}
		public void Save(Helper.FormatRSSEnum format )
		{
			//Savoir si le fichier existe deja ou pas
			if (!IsUri && !FeedIsNull)
			{
				XmlWriterSettings set = new XmlWriterSettings();
				set.NamespaceHandling = NamespaceHandling.OmitDuplicates;
				set.OmitXmlDeclaration = true;
				set.DoNotEscapeUriAttributes = false;
				set.Indent = true;
				set.NewLineChars = "\n";
				set.IndentChars = "\t";
				set.NewLineHandling = NewLineHandling.None;

				using (var writer = XmlWriter.Create(this.Uri_RSS,set))
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
