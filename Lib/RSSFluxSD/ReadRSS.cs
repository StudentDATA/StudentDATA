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
		string msg_error;

		public string Uri
		{
			get { return _uri; }
			set { _uri = value; }
		}

		public string Msg_error
		{
			get { return msg_error; }
			set { msg_error = value; }
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
            catch (XmlException e)
            {
				msg_error = "Lien Invalide : Message D'erreur : " + e.Message;
				return null;
            }
        }
	}
	
}
