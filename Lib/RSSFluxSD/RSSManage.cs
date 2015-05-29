using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD
{
	public class RSSManage
	{
		string msg_error = null;
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
				RSSList.Add(new RSS(url));
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


		public void createRSS(string url)
		{
			if (!Helper.TryUri(url))
			{
				if (!Helper.TryRSSExist(RSSList, url))
				{
					RSSList.Add(new RSS(url));
				}
				RSS u = RSSList.Find(x => x.Uri_RSS == url);
				u.InitRSS();
				//u.AddFlow();
				u.AddinXml();
			}
			else
			{
				Msg_error = "Impossible de Creer un flux RSS à partir d'un lien";
			}
		}
		/// <summary>
		/// Add a List of flow in your rss file.
		/// </summary>
		/// <param name="url">link's file</param>
		/// <param name="flow">List of flow's information : Title and Content</param>
		public void addFlow(string url,List<string> flow)
		{
			//flow contient titre et contenu : id = titre+numero du flow
			if (!Helper.TryUri(url))
			{
				if (Helper.TryRSSExist(RSSList, url))
				{
					RSS u = RSSList.Find(x => x.Uri_RSS == url);
					u.AddFlow(flow);
				}
			}
		}

		public void addFlow(string url, List<Flow> flow)
		{
			//flow contient titre et contenu : id = titre+numero du flow
			if (!Helper.TryUri(url))
			{
				if (Helper.TryRSSExist(RSSList, url))
				{
					RSS u = RSSList.Find(x => x.Uri_RSS == url);
					u.AddFlow(flow);
				}
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
					rss.AddinXml();
				};
			}
		}
		
		//Ajouter, SUPP, archives, update  : RSS



		//Ajouter , up , del
	
		//
	}
}
