using System;
using System.Collections.Generic;
using System.IO;
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

		static public bool TryRSSExist(List<RSS> RSSList, string url)
		{
			return RSSList.Exists(x => x.Uri_RSS == url);
		}

		static public bool TryFileExist(string path)
		{
			return File.Exists(path);
		}

		static public bool TryFileEmpty(string path)
		{
			if (new FileInfo( path ).Length == 0 )
			{
				return true;
			}
			else
			{
				return false;
			}

		}

		static public string EncoderString(string str)
		{
			byte[] bytes = Encoding.Default.GetBytes(str);
			return str = Encoding.Default.GetString(bytes); 
		}
		public enum CategorieRSSEnum
		{
			School,
			Etudiant
		};
	}
}
