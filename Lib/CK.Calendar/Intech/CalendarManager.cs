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
		string _teacherFind = String.Empty;
		string _filiereFind = String.Empty;

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

		public void Load(IActivityMonitor m,string semester = "ALL", bool forceReload = false)
		{
			Match match = Helper.ExtractSameMatch(ref semester, _rSemester);
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

				match = Helper.ExtractSameMatch(ref semester, _rFiliere);
				if (match != null)
				{
					_filiereFind = match.Groups[0].Value;
					m.Trace().Send("Filiere find : " + _filiereFind);
				}
				else m.Error().Send("No Filiere Find");
			}
			else
			{
				match = Helper.ExtractSameMatch(ref semester, _rTeachers);
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
						m.Error().Send("No teacher Find");
					}
				}
				else m.Error().Send("No groups Find");
			}
			
			if (forceReload || !File.Exists(FilePlanningPath))
			{
				_planning = GetHyperPlanning(m, _sClass);
				using (var s = File.OpenWrite(FilePlanningPath))
				{
					_planning.Save(s);
					teacherFind();
					filiereFind();
				}
			}
			else
			{
				using (var s = File.OpenRead(FilePlanningPath))
				{
					_planning = Planning.Load(s);
					teacherFind();
					filiereFind();
				}
			}
		}

		void teacherFind()
		{
			if (_teacherFind != String.Empty)
			{
				_planning.Teacher = _teacherFind;
				_teacherFind = String.Empty;
			}
		}

		void filiereFind()
		{
			if (_filiereFind != String.Empty)
			{
				_planning.Filiere = _filiereFind;
				_filiereFind = String.Empty;
			}
		}

        string FilePlanningPath
        {
            get 
			{ 
				if ( _sClass != StudentClass.SemesterMask )
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


    }
}
