using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD
{
	static public class Helper
	{
		static public bool TryUri(string url)
		{
			bool result;
			Uri uriResult;
			return result = Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uriResult)
				&& ( uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps );
			
		}

		static public bool TryRSSExist(List<RSS> RSSList, string url)
		{
			return RSSList.Exists(x => x.Uri_RSS == url);
		}
	}
}
