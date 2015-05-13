using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD
{
	public class Flow
	{
		string _title;
		string _id;
		string _content;
		string _url;

		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}
		public string Content
		{
			get { return _content; }
			set { _content = value; }
		}
		public string Url
		{
			get { return _url; }
			set { _url = value; }
		}
		public string Id
		{
			get { return _id; }
			set { _id = value; }
		}

		public Flow(string title,string content,string url,string id)
		{
			this.Title = title;
			this.Content = content;
			this.Url = url;
			this.Id = id;
		}
	}
}
