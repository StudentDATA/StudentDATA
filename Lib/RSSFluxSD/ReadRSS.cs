using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Drawing;
using System.Net.Mime;

namespace RSSFluxSD
{
    public class ReadRSS
    {
		string _uri;

		public string Uri
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public ReadRSS(string uri)
		{
			this.Uri = uri;
		}

		public SyndicationFeed ReadWithURI()
		{
            try
            {
                XmlReader reader = XmlReader.Create(this.Uri);
                SyndicationFeed feed = SyndicationFeed.Load(reader);
				reader.Close();
				return feed;
            }
            catch (XmlException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
				return null;
            }
        }
	}
	
}
