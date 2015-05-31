﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD
{
	public class RSSManage
	{
		string msg_error = null;
		Helper.FormatRSSEnum _formatRSS;

		public Helper.FormatRSSEnum FormatRSS
		{
			get { return _formatRSS; }
			private set { _formatRSS = value; }
		}
		public string Msg_error
		{
			get { return msg_error; }
			private set { msg_error = value; }
		}


		List<RSS> RSSList = new List<RSS>();
		public RSS readRSS(string url)
		{
			//Si même url, voir s'il y a des difference dans les flows
			if (!Helper.TryRSSExist(RSSList, url))
			{
				RSSList.Add(new RSS(url, Helper.FormatRSS20()));
			}
			RSS rss = RSSList.Find(x => x.Uri_RSS == url);
			rss.ReadRSS();
			if (rss.FeedIsNull)
			{
				RSSList.Remove(rss);
				return null;
			}
			return rss;
		}


		public RSS createRSS(string url,string title, string content, Helper.CategorieRSSEnum categorie,Helper.FormatRSSEnum formatRSS )
		{
			//Verifie si c'est un url
			if (!Helper.TryUri(url))
			{
				//Verifie si le rss existe deja dans la liste
				if (!Helper.TryRSSExist(RSSList, url))
				{
					RSSList.Add(new RSS(url, Helper.FormatRSS20()));
				}
				RSS rss = RSSList.Find(x => x.Uri_RSS == url);
				//Verifie si le fichier existe deja
				if (!Helper.TryFileExist(url))
				{
					rss.InitRSS(title, categorie, content);
					return rss;
				}
				else
				{
					if (Helper.TryFileEmpty(url))
					{
						rss.InitRSS(title, categorie, content);
					}
					//A améliorer regarder si c'est exactement le même ou un différent.
					Msg_error = "Fichier existe deja";
					return readRSS(url);
				}
			}
			else
			{
				Msg_error = "Impossible de Creer un flux RSS à partir d'un lien";
				return null;
			}
		}


		public void addFlow(string url, List<Flow> flow)
		{
			//flow contient titre et contenu : id = titre+numero du flow
			if (!Helper.TryUri(url))
			{
				if (!Helper.TryRSSExist(RSSList, url))
				{
					RSSList.Add(new RSS(url, Helper.FormatRSS20()));
				}
					RSS u = RSSList.Find(x => x.Uri_RSS == url);
					u.AddFlow(flow);
					u.Save(FormatRSS);
			}
		}

		public IReadOnlyList<RSS> GetAllRSS()
		{
			return RSSList;
		}

		public void addToXml()
		{
			foreach (RSS rss in RSSList)
			{
				if (!Helper.TryUri(rss.Uri_RSS))
				{
					rss.Save(FormatRSS);
				};
			}
		}
	}
}
