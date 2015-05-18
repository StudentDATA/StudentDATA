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
			string _titreDescription;
			string _url;
			string _author;
			string _categorie;
			string _content;

			public string Titre
			{
				get { return _titre; }
				set { _titre = value; }
			}
			public string TitreDescription
			{
				get { return _titreDescription; }
				set { _titreDescription = value; }
			}
			public string Url
			{
				get { return _url; }
				set { _url = value; }
			}
			public string Author
			{
				get { return _author; }
				set { _author = value; }
			}
			public string Categorie
			{
				get { return _categorie; }
				set { _categorie = value; }
			}
			public string Content
			{
				get { return _content; }
				set { _content = value; }
			}

			public CreateRSS()
			{
				this.Titre = "IN'TECH INFO RSS";
				this.TitreDescription = "RSS de IN'TECH INFO";
				//Gerer le url par le titre
				this.Url = "http://student-data.itinet.fr";
				this.Author = "Student DATA RSS";
				this.Categorie = "Actualité IN'TECH INFO";
				this.Content = "L'actualité de l'école IN'TECH INFO";
			}


			public CreateRSS(string titre,string titredescription, string url, string author, string categorie, string content)
			{
				this.Titre = titre;
				this.TitreDescription = titredescription;
				this.Url = url;
				this.Author = author;
				this.Categorie = categorie;
				this.Content = content;
			}

			public SyndicationFeed CreateInit()
			{
				SyndicationFeed feed = new SyndicationFeed(Titre, TitreDescription, new Uri(Url));
				feed.Authors.Add(new SyndicationPerson(Author));
				feed.Categories.Add(new SyndicationCategory(Categorie));
				feed.Description = new TextSyndicationContent(Content);
				return feed;
			}

		}



}
