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
			return result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
				&& ( uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps );
			 
		}
	}
}
