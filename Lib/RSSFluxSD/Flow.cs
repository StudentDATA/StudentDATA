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
		DateTimeOffset _date;

		public DateTimeOffset Date
		{
			get { return _date; }
			set { _date = value; }
		}

		public string Title
		{
			get 
			{
				byte[] bytes = Encoding.Default.GetBytes(_title);
				return _title = Encoding.Default.GetString(bytes); 
			}
			private set { _title = value; }
		}
		public string Content
		{
			get 
			{
				byte[] bytes = Encoding.Default.GetBytes(_content);
				return _content = Encoding.Default.GetString(bytes); 
			}
			private set { _content = value; }
		}
		public string Url
		{
			get 
			{
				byte[] bytes = Encoding.Default.GetBytes(_url);
				return _url = Encoding.Default.GetString(bytes); 
			}
			private set { _url = value; }
		}
		public string Id
		{
			get { return _id; }
			private set { _id = value; }
		}
		public Flow(string title, string content)
		{
			this.Title = title;
			this.Content = content;
			this.Url = "http://www.google.com";
			this.Id = "1";
			this.Date = DateTimeOffset.Now;
		}
		public Flow(string title, string content, string url, string id, DateTimeOffset date)
		{
			this.Title = title;
			this.Content = content;

			if (Helper.TryUri(url))
			{
				this.Url = url;
			}
			else
			{
				this.Url = "http://www.google.com";
			}
			this.Id = id;
			this.Date = date;
		}
	}
}
