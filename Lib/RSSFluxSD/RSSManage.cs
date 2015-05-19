using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD
{
	public class RSSManage
	{
		List<RSS> RSSList = new List<RSS>();

		public void addRSS(string url)
		{
			RSSList.Add(new RSS(url));
			//RSSList.LastOrDefault().ReadRSS();
			RSSList.ElementAt(0).ReadRSS();
		}

		public void createRSS(string url)
		{
			RSSList.Add(new RSS(url));
		}

		public IReadOnlyList<RSS> GetAllRSS()
		{
			return RSSList;
		}

		public void addToXml()
		{
			foreach (RSS rss in RSSList)
			{
				rss.AddinXml();
			}
		}

		//Retourner un objet rss avec ses liste de ses articles

		
		
		
		//Creation du flux rss :  retourner le nom du fichier xml

		//Ajouter, SUPP, archives, update  : RSS



		//Ajouter , up , del
	
		//
	}
}
