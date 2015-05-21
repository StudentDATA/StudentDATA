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

		//Name pour les fichiers xml : xmlName
		//Name pour les url : urlName
		List<RSS> RSSList = new List<RSS>();
		public void readRSS(string url)
		{
			RSSList.Add(new RSS(url));
			RSSList.Last().ReadRSS();
			if (RSSList.Last().FeedIsNull) RSSList.Remove(RSSList.Last());
		}

		public void createRSS(string url)
		{
			if ( !Helper.TryUri(url))
			{
				RSSList.Add(new RSS(url));
				RSSList.Last().InitRSS();
				RSSList.Last().AddFlow();
				addToXml();
			}
			else
			{
				Msg_error = "Impossible de Creer un flux RSS à partir d'un lien";
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
