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
       // readonly List<SchoolEvent> _events;
		List<SchoolEvent> _events;
		string _teacher;
		string _filiere;

	
		internal void DeleteEvent(SchoolEvent e)
		{
			_events.Remove(e);
		}


		public string Teacher
		{
			get { return _teacher; }
			internal set { _teacher = value; }
		}
		public string Filiere
		{
			get { return _filiere; }
			internal set { _filiere = value; }
		}

        internal Planning( List<SchoolEvent> events )
        {
            _events = events;
			_teacher = String.Empty;
			_filiere = String.Empty;
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
			get 
			{
				Func<SchoolEvent, bool> _filter = null;

				if (_teacher != String.Empty)
				{
					_filter = TeachersFilter;
				}
				else if ( _filiere != String.Empty)
				{
					if( _filiere == "IL")
					{
						_filiere = String.Empty;
						_filter = ILFilter;
					}
					else if (_filiere == "SR")
					{
						_filiere = String.Empty;
						_filter = SRFilter;
					}
				}
				if ( _filter != null)
				{
					return _events.Where(_filter).OrderBy(x => x.Beg);
				}
				else return _events.OrderBy(x => x.Beg); 
			}
        }

		public IEnumerable<SchoolEvent> EventsByDate
		{
			get { return _events.OrderBy(x => x.Beg); }
		}
        [Obsolete]
		public IEnumerable<SchoolEvent> EventsIL
		{
			get { return _events.Where(ILFilter).OrderBy(x => x.Beg); }
		}
        [Obsolete]
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

		bool TeachersFilter(SchoolEvent e)
		{
			if (_teacher == "SPINELLI" || _teacher == "RAQUILLET")
			{
				return e.Teachers.Contains(_teacher)
						|| (e.Classes.Any(c => (c & StudentClass.S03IL) == StudentClass.S03IL) && (e.Title.Contains("Projet informatique") || (e.Code == "IT-PIN-3-2") ||  e.Code== "IT-PIN-3-1"))
						|| (e.Classes.Any(c => (c & StudentClass.S04IL) == StudentClass.S04IL) && (e.Title.Contains("Projet informatique") || (e.Code == "IT-PIN-4-2") || e.Code == "IT-PIN-4-1"))
						|| (e.Classes.Any(c => (c & StudentClass.S05IL) == StudentClass.S05IL) && (e.Title.Contains("Projet informatique") || (e.Code == "IT-PIN-5-2") || e.Code == "IT-PIN-5-1"));
			}
			else if (_teacher == "KOUDOSSOU")
			{
				return e.Teachers.Contains(_teacher)
						|| (e.Classes.Any(c => (c & StudentClass.S03SR) == StudentClass.S03SR) && (e.Title.Contains("Projet informatique") || (e.Code == "IT-PIN-3-2") || e.Code == "IT-PIN-3-1"))
						|| (e.Classes.Any(c => (c & StudentClass.S04SR) == StudentClass.S04SR) && (e.Title.Contains("Projet informatique") || (e.Code == "IT-PIN-4-2") || e.Code == "IT-PIN-4-1"))
						|| (e.Classes.Any(c => (c & StudentClass.S05SR) == StudentClass.S05SR) && (e.Title.Contains("Projet informatique") || (e.Code == "IT-PIN-5-2") || e.Code == "IT-PIN-5-1"));
			}
			else if (_teacher == "DORIGNAC")
			{
				return e.Teachers.Contains(_teacher)
						|| (e.Classes.Any(c => (c & StudentClass.S02) == StudentClass.S02) && (e.Title.Contains("PFH") || e.Code == "IT-PFH-2-2"))
						|| (e.Classes.Any(c => (c & StudentClass.S03) == StudentClass.S03) && (e.Title.Contains("PFH") || e.Code == "IT-PFH-3-2"))
						|| (e.Classes.Any(c => (c & StudentClass.S04) == StudentClass.S04) && (e.Title.Contains("PFH") || e.Code == "IT-PFH-4-2"));
			}
			else if (_teacher == "GOT")
			{
				return e.Teachers.Contains(_teacher)
						|| (e.Classes.Any(c => (c & StudentClass.S05) == StudentClass.S05) && (e.Title.Contains("PFH") || (e.Code == "IT-PFH-5-2" || e.Code == "IT-PFH-5-1")));
			}
			else if (_teacher == "SANCHEZ" || _teacher == "DANESI")
			{
				return e.Teachers.Contains(_teacher)
						|| (e.Classes.Any(c => (c & StudentClass.S01) == StudentClass.S01) && (e.Title.Contains("PFH") || (e.Code == "IT-PFH-1-2" || e.Code == "IT-PFH-1-1")));
			}
			else if (_teacher == "THIRE")
			{
				return e.Teachers.Contains(_teacher)
						|| (e.Classes.Any
							(c => (c & StudentClass.S01) == StudentClass.S01) 
							&& ( (e.Title.Contains("Programmation") || e.Title.Contains("Projet informatique")) 
							|| (e.Code == "IT-PRG-1-2" || e.Code == "IT-PRG-1-1") || (e.Code == "IT-PIN-1-2" || e.Code == "IT-PIN-1-1")));
			}
			return e.Teachers.Contains(_teacher);


			//Faire pareil pour TOUS les prof IL et SR
		}

	}
}
