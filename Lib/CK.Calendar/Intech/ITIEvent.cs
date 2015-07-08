using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CK.Calendar.Intech
{
	[Serializable]
	public class ITIEvent
	{
		protected string _location;
		protected DateTime _beg;
		protected DateTime _end;
		protected string _code;
		protected string _title;
		private string[] _organizer;

		public ITIEvent(string subjectTitle,
			string[] organizer,
			string location,
			DateTime beg,
			DateTime end)
		{
			_title = subjectTitle;
			_organizer = organizer;
			_location = location;
			_beg = beg;
			_end = end;
			_code = this.GetHashCode().ToString();
		}

		public ITIEvent() { }
		public string[] Organizer
		{
			get { return _organizer; }
			internal set { _organizer = value; }
		}

		public string Location
		{
			get { return _location; }
			internal set { _location = value; }
		}

		public DateTime Beg
		{
			get { return _beg; }
			internal set { _beg = value; }
		}

		public TimeSpan Length
		{
			get { return _end - _beg; }
		}

		public DateTime End
		{
			get { return _end; }
			internal set { _end = value; }
		}

		public String BegToString
		{
			get { return _beg.ToLocalTime().ToString(); }
		}

		public String LenghtToString
		{
			get { return Length.Hours + ":" + Length.Minutes.ToString("00"); }
		}

		public String EndToString
		{
			get { return _end.ToLocalTime().ToString(); }
		}

		public string Code
		{
			get { return _code; }
		}

		public string Title
		{
			get { return _title; }
			internal set { _title = value; }
		}
	}
}
