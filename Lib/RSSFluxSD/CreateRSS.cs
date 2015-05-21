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
			string _titre;
			string _url;
			string _author;
			string _categorie;
			string _content;

			public string Titre
			{
				get { return _titre; }
				private set { _titre = value; }
			}

			public string Url
			{
				get { return _url; }
				private set { _url = value; }
			}
			public string Author
			{
				get { return _author; }
				private set { _author = value; }
			}
			public string Categorie
			{
				get { return _categorie; }
				private set { _categorie = value; }
			}
			public string Content
			{
				get { return _content; }
				private set { _content = value; }
			}

			public enum CategorieEnum 
			{
				School, 
				Etudiant
			};


			public void InitRSSBase()
			{
				this.Titre = "IN'TECH INFO RSS";
				this.Url = "http://student-data.itinet.fr";
				this.Author = "Student DATA RSS";
				this.Categorie = CategorieEnum.Etudiant.ToString();
				this.Content = "L'actualité de l'école IN'TECH INFO";
			}

			public CreateRSS()
			{

			}

			public CreateRSS(string titre, string titredescription, string categorie, string content)
			{
				this.Titre = titre;
				this.Url = "http://student-data.itinet.fr";
				this.Author = "Student DATA RSS";
				this.Categorie = categorie;
				this.Content = content;
			}

			public void AddInitRSS(string titre, string url, string author, string categorie, string content)
			{
				this.Titre = titre;
				this.Url = url;
				this.Author = author;
				this.Categorie = categorie;
				this.Content = content;
			}

			public SyndicationFeed CreateInit()
			{
				SyndicationFeed feed = new SyndicationFeed(Titre, Titre, new Uri(Url));
				feed.Authors.Add(new SyndicationPerson(Author));
				feed.Categories.Add(new SyndicationCategory(Categorie));
				feed.Description = new TextSyndicationContent(Content);
				return feed;
			}

		}



}
