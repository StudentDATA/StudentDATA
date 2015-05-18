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

		public void addRSS()
		{
			RSSList.Add(new RSS("text.xml"));
		}

		public IReadOnlyList<RSS> GetAllRSS()
		{
			addRSS();
			return RSSList;
		}

		//Retourner un objet rss avec ses liste de ses articles

		//Creation du flux rss :  retourner le nom du fichier xml

		//Ajouter, SUPP, archives, update  : RSS



		//Ajouter , up , del
	
		//
	}
}
