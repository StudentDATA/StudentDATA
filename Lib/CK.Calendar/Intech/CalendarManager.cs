﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CK.Core;
using DDay.iCal;

namespace CK.Calendar.Intech
{
    public class CalendarManager
    {
        static readonly string[] _teacherNames = new string[] { 
            "ALEXIADE", 
            "BLOCH", 
            "BONNET", 
            "BOULANGER", 
            "BROUSTE", 
            "COCKS", 
            "DORIGNAC", 
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
            "KOUDOSSOU", 
            "LALITTE", 
            "PUCHAUX",
			"RAQUILLET",
            "SOULEZ",
            "SPINELLI", 
            "SPY",
            "TALAVERA", 
            "THOMAS", 
        };

		static IDictionary<StudentClass, string> _hyperplanningacces;

        readonly string _dbPath;
        Planning _planning;
		StudentClass _sClass;

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

		void Log_To_File(string msg)
		{
			DateTimeOffset _date = DateTimeOffset.Now;
			string _nameFile = _date.Day.ToString() + "_" + _date.Month.ToString() + "_" + _date.Year.ToString() + " Calendar.log";
			msg = Environment.NewLine + _date.ToString() + Environment.NewLine + msg;
			File.AppendAllText(Path.Combine(Path.Combine(_dbPath, "Logs"), _nameFile), msg);
		}

		public void Load(IActivityMonitor m,string semester = "ALL", bool forceReload = false)
		{
			Action<string> _logAction = Log_To_File;    
			ActivityMonitorTextWriterClient _log = new ActivityMonitorTextWriterClient(_logAction);
			m.Output.RegisterClients(_log);

			if (semester == "S01") _sClass = StudentClass.S01;
			else if (semester == "S02") _sClass = StudentClass.S02;
			else if (semester == "S02") _sClass = StudentClass.S02;
			else if (semester == "S03") _sClass = StudentClass.S03;
			else if (semester.Contains("S04")) _sClass = StudentClass.S04;
			else if (semester.Contains("S05")) _sClass = StudentClass.S05;
			else if (semester.Contains("S07") || semester.Contains("S08")) _sClass = StudentClass.S07;
			else if (semester.Contains("S09") || semester.Contains("S10")) _sClass = StudentClass.S09;
			else _sClass = StudentClass.SemesterMask;

			if (forceReload || !File.Exists(FilePlanningPath))
			{
				_planning = GetHyperPlanning(m, _sClass);
				using (var s = File.OpenWrite(FilePlanningPath))
				{
					_planning.Save(s);

					m.Output.UnregisterClient(_log);
				}
			}
			else
			{
				using (var s = File.OpenRead(FilePlanningPath))
				{
					_planning = Planning.Load(s);
					m.Output.UnregisterClient(_log);
				}
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