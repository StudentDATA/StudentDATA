using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;
using DDay.iCal;
using System.Text.RegularExpressions;

namespace CK.Calendar.Intech
{
    public class CalendarManager
    {

		static readonly Regex _rSemester = new Regex(@"^S(?<1>10|0[1-9])", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
		static readonly Regex _rFiliere = new Regex(@"(SR)|(IL)", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
		static readonly Regex _rTeachers = new Regex(@"^[a-zA-Z]+$", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);
		static readonly Regex _rPerso = new Regex(@"^Perso-(?<1>[a-zA-Z0-9]+$)", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        static readonly string[] _teacherNames = new string[] { 
            "ALEXIADE", 
            "BLOCH", 
            "BONNET", 
            "BOULANGER", 
            "BROUSTE", 
            "COCKS", 
            "DORIGNAC", 
			"DANESI",
            "FARCY",
            "FIALEK",
            "FRANCESHI",
			"FRIPIAT",
            "FRADET", 
            "GOT", 
            "GOY", 
            "GROS", 
            "HUET",
            "JOUBERT",
            "JOUVENT",
			"Adjevi KOUDOSSOU",
            "KOUDOSSOU", 
            "LALITTE", 
			"MARSAUD",
            "PUCHAUX",
			"RAQUILLET",
            "SOULEZ",
			"SANCHEZ",
            "SPINELLI", 
            "SPY",
            "TALAVERA", 
            "THOMAS", 
        };

		static IDictionary<StudentClass, string> _hyperplanningacces;

        readonly string _dbPath;
        Planning _planning;
		StudentClass _sClass;
		IActivityMonitor _iAmonitor;
		string _teacherFind = String.Empty;
		string _filiereFind = String.Empty;
		string _persoFind = String.Empty;
		bool _eventITIfind;

        public CalendarManager( string dbPath )
        {
            _dbPath = dbPath;
			_hyperplanningacces = new Dictionary<StudentClass, string>();
			_hyperplanningacces.Add(StudentClass.S01, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S01___2015M.ics?version=13.0.2.0&idICal=4CF01083AC2FB4CA1AB539D2AF66F447&param=643d5b312e2e36325d2666683d3126663d3131303030");
			_hyperplanningacces.Add(StudentClass.S02, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S02___2015M.ics?version=13.0.2.0&idICal=A433C2BB6F31B3A3CE43D0B00E9BAC50&param=643d5b312e2e36325d2666683d3126663d3131303030");
			_hyperplanningacces.Add(StudentClass.S03, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S03___2015M.ics?version=13.0.2.0&idICal=4F32066ED19E0AE665DE2AE342F5D9B5&param=643d5b312e2e36325d2666683d3126663d3131303030");
			_hyperplanningacces.Add(StudentClass.S04, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S04___2015M.ics?version=13.0.2.0&idICal=21F6195C5A0B152295A76620F7063C66&param=643d5b312e2e36325d2666683d3126663d3131303030");
			_hyperplanningacces.Add(StudentClass.S05, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S05___2015M.ics?version=13.0.2.0&idICal=2B6BF0FA8E30EE3A2E0C181F48808C11&param=643d5b312e2e36325d2666683d3126663d3131303030");
			_hyperplanningacces.Add(StudentClass.S07, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_A4M___2015M.ics?version=13.0.2.0&idICal=9D7B33CEBF8171D38E5D132AC1EEAD9D&param=643d5b312e2e36325d2666683d3126663d3131303030");
			_hyperplanningacces.Add(StudentClass.S09, @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_A5M___2015M.ics?version=13.0.2.0&idICal=4DD02635471974E0BE5F0B7460C6518F&param=643d5b312e2e36325d2666683d3126663d3131303030");

        }

        public Planning Planning
        {
            get { return _planning; }
        }

		public void Load(IActivityMonitor m,string calendarName = "ALL", bool forceReload = false)
        {
			_iAmonitor = m;
			_eventITIfind = false;
			_persoFind = String.Empty;


			Match match = Helper.ExtractSameMatch(ref calendarName, _rSemester);
			if ( match != null )
			{
				var resultMatch = match.Groups[0].Value;
				var resultMatch2 = match.Groups[1].Value;
				var resultMatch3 = match.Groups[2].Value;
				if (resultMatch == "S01") _sClass = StudentClass.S01;
				else if (resultMatch == "S02") _sClass = StudentClass.S02;
				else if (resultMatch == "S02") _sClass = StudentClass.S02;
				else if (resultMatch == "S03") _sClass = StudentClass.S03;
				else if (resultMatch == "S04") _sClass = StudentClass.S04;
				else if (resultMatch == "S05") _sClass = StudentClass.S05;
				else if (resultMatch == "S07" || resultMatch == "S08") _sClass = StudentClass.S07;
				else if (resultMatch == "S09" || resultMatch == "S10") _sClass = StudentClass.S09;
				else _sClass = StudentClass.SemesterMask;
				m.Trace().Send( "Semester find : " + resultMatch);

				match = Helper.ExtractSameMatch(ref calendarName, _rFiliere);
				if (match != null)
				{
					_filiereFind = match.Groups[0].Value;
					m.Trace().Send("Filiere find : " + _filiereFind);
				}
				else m.Error().Send("No Filiere Find");
			}
			else if ( calendarName == "EventITI" )
			{
				_eventITIfind = true;
			}
			else
			{
				match = Helper.ExtractSameMatch(ref calendarName, _rPerso);
				if ( match != null )
				{
					_persoFind = match.Groups[1].Value;
				}
				else
				{
					match = Helper.ExtractSameMatch(ref calendarName, _rTeachers);
					if ( match != null)
					{
						var resultMatch = match.Groups[0].Value;
						_sClass = StudentClass.SemesterMask;
						foreach (var t in _teacherNames)
						{
							if ( resultMatch == t)
							{
								_teacherFind = resultMatch;
							}
						}
						if ( _teacherFind == String.Empty)
						{
							m.Error().Send("No teacher Find : {0}", calendarName);
						}

					}
					m.Error().Send("No Perso Or EventITI Or Teachers Find : {0}", calendarName);
				}
				//else m.Error().Send("No groups Find");
			}
			
			if (forceReload || !File.Exists(FilePlanningPath))
			{
				_planning = GetHyperPlanning(m, _sClass);
				using (var s = File.OpenWrite(FilePlanningPath))
				{
					_planning.Save(s);
					filterFind();
				}
			}
			else
			{
				using (var s = File.OpenRead(FilePlanningPath))
				{
					_planning = Planning.Load(s);
					filterFind();
				}
			}
		}

		void filterFind()
		{
			if (_teacherFind != String.Empty)
			{
				_planning.Teacher = _teacherFind;
				_teacherFind = String.Empty;
			}
			if (_filiereFind != String.Empty)
			{
				_planning.Filiere = _filiereFind;
				_filiereFind = String.Empty;
			}
		}

        string FilePlanningPath
        {
			//A FACTORISER
            get 
			{ 
				if ( _sClass == 0)
				{
					if (_eventITIfind) return Path.Combine(_dbPath, "Event-ITI-Calendar.bin");
					else if (_persoFind != String.Empty ) return Path.Combine(_dbPath, "Personal-Caldendar-SD-" + _persoFind + ".bin");
					else return Path.Combine(_dbPath, "ErrorPlanning.bin");
				}
				else if ( _sClass != StudentClass.SemesterMask )
				{
					return Path.Combine(_dbPath, _sClass.ToExplicitString() + "_Planning.bin");
				}
				else
				{
					return Path.Combine( _dbPath, "Planning.bin" ); 
					
				}

			}
        }

		Planning GetHyperPlanning(IActivityMonitor m, StudentClass defaultClass)
		{
			List<SchoolEvent> events = new List<SchoolEvent>();
			if ( defaultClass != 0)
			{
				foreach (KeyValuePair<StudentClass, string> kvp in _hyperplanningacces)
				{
					//REFAIRE
					if ( defaultClass == StudentClass.SemesterMask)
					{
						GetData(events, m, kvp.Key, kvp.Value);
					}
					else if ( defaultClass == kvp.Key)
					{
						GetData(events, m, kvp.Key, kvp.Value);
					}	
				}
			}
			return new Planning(events);
		}

	
        void GetData( List<SchoolEvent> events, IActivityMonitor m, StudentClass sc, string uri )
        {
            var all = iCalendar.LoadFromUri( new Uri( uri ) );
            foreach( var c in all )
            {
                foreach( var e in c.Events )
                {
					string _date = DateTimeOffset.Now.Year.ToString();
					var u = DateTimeOffset.Now.Month;
					if ( u >= 3 && u <= 8)
					{
						_date += "M";
					}
					else
					{
						_date += "S";
					}

                    var ie = SchoolEvent.BuildFrom( m, e, sc, _teacherNames, _date );
                    if( ie != null )
                    {
                        events.Add( ie );
                    }
                }
            }
        }

		public void AddData(string title, Dictionary<string, string> organizer, string location, DateTime beg, DateTime end)
		{
			if (calendarSDVerif() && DataVerif(title,organizer,location,beg,end))
			{
				List<SchoolEvent> events = new List<SchoolEvent>();
				//Gerer les dates,et autres erreurs
				if (_planning.Events.Count() > 0) events.AddRange(_planning.Events);
				var ie = new SchoolEvent(title, organizer, location, beg, end);
				if ( ie != null )
				{
					events.Add(ie);
					_planning = new Planning(events);
				}
				else
				{
					_iAmonitor.Error().Send("No Events");//Lister error
				}
			}

		}

		public void RemoveData(SchoolEvent e)
		{
			if (calendarSDVerif())
			{
				_planning.DeleteEvent(e);
			}
		}

		public void RemoveData(string code)
		{
			if (calendarSDVerif())
			{
				_planning.DeleteEvent(_planning.Events.Where(x => x.Code == code).FirstOrDefault());
			}
		}

		public void UpDateData(SchoolEvent e,string title,string location, Dictionary<string,string> organizer, DateTime beg, DateTime end)
		{
			if (calendarSDVerif() && DataVerif(title, organizer, location, beg, end))
			{
				var ie = _planning.Events.SingleOrDefault(x => x.Code == e.Code);

				if (ie != null)
				{
					ie.Title = title;
					ie.Location = location;
					ie.Organizer = organizer;
					ie.Beg = beg;
					ie.End = end;
				}
			}
		}

		public void SaveData()
		{
			if ( _planning != null )
			{
				using (var s = File.OpenWrite(FilePlanningPath))
				{
					_planning.Save(s);
				}
			}
			else
			{
				_iAmonitor.Error().Send("No Planning");
			}
		}

		public bool calendarSDVerif()
		{
			if (_persoFind != String.Empty || _eventITIfind)
			{
				return true;
			}
			else return false;
		}

		public bool DataVerif(string title, Dictionary<string, string> organizer, string location, DateTime beg, DateTime end)
		{
			//A REFAIRE
			if (title == null) return false;

			//Qu'il y a le nom et l'email
			if (organizer == null) return false;
			//Location, Lettre , chiffres , accents
			if (location == null) return false;

			//Que les doit soit correcte entre début 2015 et fin 2020 et que end soit supérieur à beg 
			if (beg > end) return false;
			return true;

		}

    }
}
