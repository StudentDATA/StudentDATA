using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RSSFluxSD
{
	public class Article
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
				return Helper.EncoderString(_title);
			}
			internal set { _title = value; }
		}
		public string Content
		{
			get 
			{
				return Helper.EncoderString(_content);
			}
			internal set { _content = value; }
		}
		public string Url
		{
			get 
			{
				return Helper.EncoderString(_url);
			}
			private set { _url = value; }
		}
		public string Id
		{
			get { return _id; }
			private set { _id = value; }
		}
		public Article(string title, string content)
		{
			this.Title = title;
			this.Content = content;
			this.Url = "http://www.google.com/";
			this.Date = DateTimeOffset.Now;
			updateID();
			
		}
		public Article(string title, string content, string url, string id, DateTimeOffset date)
		{
			this.Title = title;
			this.Content = content;

			if (Helper.TryUri(url))
			{
				this.Url = url;
			}
			else
			{
				this.Url = "http://www.google.com/";
			}
			this.Id = id;
			this.Date = date;
		}

		public void updateID()
		{
			string md5ID = this.GetHashCode().ToString();
			this.Id = "";
			using (MD5 md5Hash = MD5.Create())
			{
				this.Id = Helper.mD5Hash(md5Hash, md5ID);
			}
		}
	}
}
