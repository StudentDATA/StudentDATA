using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CK.Core;
using DDay.iCal;

namespace CK.Calendar.Intech
{
    [Serializable]
	public class SchoolEvent : ITIEvent
    {
        StudentClass[] _classes;
		//string _description;
        string[] _teachers;


        SchoolEvent( StudentClass[] classes,
                    string subjectCode,
                    string subjectTitle,
                    string[] teachers,
                    string location,
                    DateTime beg,
                    DateTime end  )
        {
            _classes = classes;
            _code = subjectCode;
            _title = subjectTitle;
            _teachers = teachers;
            _location = location;
            _beg = beg;
            _end = end;
        }

		public SchoolEvent(string subjectTitle,
			string[] teachers,
			string location,
			DateTime beg,
			DateTime end)
		{
			_title = subjectTitle;
			_teachers = teachers;
			_location = location;
			_beg = beg;
			_end = end;
			_code = this.GetHashCode().ToString();
		}

		
        static readonly Regex _rClass = new Regex( @"[0-9]{4}(S|M)-(S(?<1>10|0[1-9])|A(?<2>4|5))-(?<3>IL|SR)", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture |RegexOptions.Compiled );
        static readonly Regex _rClassIntechA = new Regex( @"INTECH-A(?<1>4|5)S", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture |RegexOptions.Compiled );
        static readonly Regex _rSubjectCode = new Regex( @"(?<1>IT-(\S)+)", RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Compiled );

        /// <summary>
        /// Initializes an <see cref="SchoolEvent"/> from a iCal event.
        /// Returns null on vacation or if an error occured.
        /// </summary>
        /// <param name="m">The monitor. Must not be null.</param>
        /// <param name="e">The iCal event.</param>
        /// <param name="defaultClass">Default class used when detailed class does not appear in the event's summary.</param>
        /// <param name="teacherNames">Names of the available teachers (will be extracted from the summary).</param>
        /// <returns>An event or null on vacation or if an error occured.</returns>
        public static SchoolEvent BuildFrom( IActivityMonitor m, IEvent e, StudentClass defaultClass, IEnumerable<string> teacherNames, string currentSemester )
        {
            using( m.OpenTrace().Send( "Building from Summary: '{0}'", e.Summary ) )
            {
                m.Trace().Send( () => String.Format( "Description: {0}", e.Description != null ? Regex.Replace( e.Description, @"(\r)?\n", Environment.NewLine ) : "(null)" ) );
                bool isValidSubjectTitle;
                bool isValidLocation;
                bool isValidDate;
                string summary = e.Summary;
                if( summary == null )
                {
                    m.Error().Send( "Missing event summary." );
                }
                if( summary == "Férié" || summary == "Vacances" )
                {
                    m.CloseGroup( "Vacation." );
                    return null;
                }
                string all = e.Description + " " + e.Summary;
                try
                {
                    StudentClass[] sc = new StudentClass[] { defaultClass };
                    string subjectCode = null;
                    string subjectTitle = null;
                    string location = e.Location;
					//verifier pour les S07/S08 ET S09/S010 : ENCORE
					Match match = Helper.ExtractSameMatch(ref all, _rClass);
                    if( match != null )
                    {
                        bool isIL = match.Groups[3].Value == "IL";
                        bool isSemester = match.Groups[1].Length > 0;
                        if( isSemester )
                        {
                            int semester = Int32.Parse( match.Groups[1].Value );
                            if( isIL ) semester += (int)StudentClass.IL;
                            else semester += (int)StudentClass.SR;
                            sc[0] = (StudentClass)semester;
                        }
                        else
                        {
                            Debug.Assert( match.Groups[2].Value == "4" || match.Groups[2].Value == "5" );
                            int year = Int32.Parse( match.Groups[2].Value );
                            int semester;
                            if( year == 4 ) semester = 7;
                            else semester = 9;
                            if( isIL ) semester += (int)StudentClass.IL;
                            else semester += (int)StudentClass.SR;
                            sc = new StudentClass[]{ (StudentClass)semester, (StudentClass)semester+1 };
                        }
                    }
                    else
                    {
						match = Helper.ExtractSameMatch(ref all, _rClassIntechA);
                        if( match != null )
                        {
                            int year = Int32.Parse( match.Groups[1].Value );
                            if( year == 4 )
                            {
                                sc = new StudentClass[] { StudentClass.S07, StudentClass.S08 };
                            }
                            else sc = new StudentClass[] { StudentClass.S09, StudentClass.S10 };
                        }
                    }
                    match = Helper.ExtractSameMatch( ref all, _rSubjectCode );
                    if( match != null )
                    {
                        subjectCode = match.Groups[1].Value;
                    }
                    else subjectCode = String.Empty;
                    List<string> teachers = new List<string>();
                    if( teacherNames != null )
                    {
                        foreach( var t in teacherNames )
                        {
                            int idx = all.IndexOf( t );
                            if( idx >= 0 )
                            {
                                teachers.Add( t );
                            }
                        }
                        foreach( var t in teachers )
                        {
                            all = all.Replace( t, String.Empty );
                        }
                    }
                    isValidLocation = !String.IsNullOrWhiteSpace( location );
                    if( !isValidLocation )
                    {
                        m.Warn().Send( "Empty location." );
                        location = String.Empty;
                    }
                    else all = all.Replace( location, String.Empty );
                    all = all.Replace( currentSemester, String.Empty );
                    all = all.Replace( "Enseignant :", String.Empty );
                    all = all.Replace( "Enseignants :", String.Empty );
                    all = all.Replace( "Mémo :", String.Empty );
                    all = all.Replace( "Matière :", String.Empty );
                    all = all.Replace( "Salle :", String.Empty );
                    all = all.Replace( "Salles :", String.Empty );
                    all = all.Replace( "TD :", String.Empty );
                    all = all.Replace( "Promotion :", String.Empty );
                    
                    subjectTitle = Regex.Replace( all, @"(\s|,|/|-)+", " " );
                    var sub1 = subjectTitle.Substring( 0, subjectTitle.Length / 2 ).Trim();
                    var sub2 = subjectTitle.Substring( subjectTitle.Length / 2 ).Trim();
                    if( sub1 != sub2 )
                    {
                        m.Warn().Send( "Title may contain duplicate data." );
                    }
                    else subjectTitle = sub1;
                    isValidSubjectTitle = !String.IsNullOrWhiteSpace( subjectTitle );
                    isValidDate = e.Start != null
                                        && e.Start.Value > Util.UtcMinValue
                                        && e.End != null && e.End.Value > e.Start.Value;

                    if( isValidSubjectTitle && isValidDate )
                    {
                        var se = new SchoolEvent( sc, subjectCode, subjectTitle, teachers.ToArray(), e.Location, e.Start.Value, e.End.Value );
                        m.Trace().Send( se.ToString() );
                        return se;
                    }
                }
                catch( Exception ex )
                {
                    m.Error().Send( ex );
                    return null;
                }
                if( !isValidDate )
                {
                    m.Error().Send( "Invalid date range." );
                }
                if( !isValidSubjectTitle )
                {
                    m.Error().Send( "Invalid description: '{0}'.", e.Description);
                    if( !isValidSubjectTitle )
                    {
                        m.Error().Send( "Unable to extract title." );
                    }
                }
                return null;
            }
        }

        public IReadOnlyList<StudentClass> Classes
        {
            get { return _classes; }
        }



        public IReadOnlyList<string> Teachers 
        { 
            get { return _teachers; }
			internal set { _teachers = value.ToArray(); }
        }



		public String ClassesToString
		{
			get { return String.Join( ", ", _classes.Select( c => c.ToExplicitString() ));}
		}

		public String TeachersToString
		{
			get { return String.Join(", ", _teachers); }
		}

        public override string ToString()
        {
            StringBuilder b = new StringBuilder();
			b.Append("Date Start: ").Append(BegToString).AppendLine();
			b.Append("Duration: ").Append(LenghtToString).AppendLine();
			b.Append("Date End: ").Append(EndToString).AppendLine();
			b.Append( "Classes: " ).Append( ClassesToString ).AppendLine();
            b.Append( "Code: " ).Append( _code ).AppendLine();
            b.Append( "Title: " ).Append( _title ).AppendLine();
			if ( _location != String.Empty && _location != null) b.Append("Location : ").Append(_location).AppendLine();
			if (_teachers.FirstOrDefault() != String.Empty && _teachers.FirstOrDefault() != null)
			{
				if ( _teachers.Count() > 1)
				{
					b.Append("Teachers: ");
				}
				else
				{
					b.Append("Teacher: ");
				}
				b.Append(TeachersToString);
				b.AppendLine();
			}
            return b.ToString();
        }

		public void Change(string title,string location, string[] organizer, DateTime beg, DateTime end)
		{
			_title = title;
			_location = location;
			_teachers = organizer;
			_beg = Beg;
			_end = end;
		}
	}
}
