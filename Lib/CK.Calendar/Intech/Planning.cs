using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CK.Calendar.Intech
{
    public class Planning
    {
        readonly List<SchoolEvent> _events;

        internal Planning( List<SchoolEvent> events )
        {
            _events = events;
        }

        public void Save( Stream s )
        {
            BinaryFormatter f = new BinaryFormatter();
            f.Serialize( s, _events );
        }

        public static Planning Load( Stream s )
        {
            BinaryFormatter f = new BinaryFormatter();
            List<SchoolEvent> ev = (List<SchoolEvent>)f.Deserialize( s );
            return new Planning( ev );
        }

        public IEnumerable<SchoolEvent> Events
        {
			get { return _events; }
        }

		public IEnumerable<SchoolEvent> EventsByDate
		{
			get { return _events.OrderBy(x => x.Beg); }
		}

		public IEnumerable<SchoolEvent> EventsIL
		{
			get { return _events.Where(ILFilter).OrderBy(x => x.Beg); }
		}

		public IEnumerable<SchoolEvent> EventsSR
		{
			get { return _events.Where(SRFilter).OrderBy(x => x.Beg); }
		}

		bool ILFilter(SchoolEvent e)
		{
			if (e.ClassesToString.Contains("SR"))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		bool SRFilter(SchoolEvent e)
		{
			if (e.ClassesToString.Contains("IL"))
			{
				return false;
			}
			else
			{
				return true;
			}
		}
    }
}
