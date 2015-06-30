using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CK.Calendar.Intech;
using DDay.iCal;
using NUnit.Framework;

namespace CK.Calendar.Tests
{

    [TestFixture]
    public class IntechParsing
    {
        // Description:	"TD : 2014S-S03-IL\nMatière : IT-PRG-3-4 - POO avec C# .Net\nEnseignant : SPINELLI\nSalle : E08-Ivry\n"
        // Summary:	"2014S-S03-IL - IT-PRG-3-4 - POO avec C# .Net - SPINELLI"
        // Summary:	"IT-DIV01 - Accueil - KOUDOSSOU, LALITTE"
        [Test]
        public void parse_event_summary()
        {

			var all = iCalendar.LoadFromUri( new Uri( @"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S05___2015M.ics?version=13.0.1.2&idICal=2B6BF0FA8E30EE3A2E0C181F48808C11&param=643d5b312e2e36325d2666683d3126663d3131303030" ) );
			var events = new List<SchoolEvent>();
            foreach( var c in all )
            {
				var eventCal = c.Events.OrderBy(x => x.Start.Value);
				foreach (var e in eventCal)
                {
					var ie = SchoolEvent.BuildFrom(TestHelper.ConsoleMonitor, e, StudentClass.S05, null, "2015M");
                    if( ie != null )
                    {
                        events.Add( ie );
                    }
                }
            }
			foreach ( var ei in events )
			{
				//Teachers isn't supported without the Manager, the manager is a String Empty because we have no list of teachers
				//Console.Write(ei.Teachers);
				Assert.AreEqual(ei.Teachers, String.Empty);
			}
			
        }


        [Test]
        [Explicit]
        public void get_fresh_planning_and_cache_it()
        {
            {
                CalendarManager m = new CalendarManager( TestHelper.CacheFolder );
                Assert.That( m.Planning == null );
                m.Load( TestHelper.ConsoleMonitor);
                Assert.That( m.Planning != null );
            }
			//Open the planning.bin and just read, CAN'T create if doesn't exist.
            {
                CalendarManager m = new CalendarManager( TestHelper.CacheFolder );
                m.Load( TestHelper.ConsoleMonitor);
                Assert.That( m.Planning != null );
            }
        }

		[Test]
		[TestCase("S01")]
		[TestCase("S02")]
		[TestCase("S03")]
		[TestCase("S04")]
		[TestCase("S05")]
		[TestCase("S05-IL")]
		[TestCase("S07")]
		[TestCase("S09")]
		[Explicit]
		public void get_fresh_planning_by_semester_and_cache_it(string semester)
		{
			{
				//Binaire Moins lourd
				//Monitor Fichier LOGS FAIT ( A METTRE DANS DOSSIER LOG DU DOSSIER PLANNING) ERROR QUAND DOSSIER PAS CREER
				CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
				Assert.That(m.Planning == null);
				//TOUS CE JOUE LA
				m.Load(TestHelper.ConsoleMonitor, semester,true);
				Assert.That(m.Planning != null);
			}
			{
				CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
				m.Load(TestHelper.ConsoleMonitor, semester);
				Assert.That(m.Planning != null);
			}
			//Rajouter dans un bin par semeste/fileres
			//Si bin existe deja, regarder les différences et les changer

		}

		//Filtrer par professor/Matieres
        bool SpiFilter( SchoolEvent e )
        {
            return e.Teachers.Contains( "SPINELLI" )
                    || (e.Classes.Any( c =>  (c & StudentClass.S03IL) == StudentClass.S03IL ) && (e.Title.Contains( "Projet informatique" ) || e.Code == "IT-PIN-3-2") )
                    || (e.Classes.Any( c => (c & StudentClass.S04IL) == StudentClass.S04IL ) && (e.Title.Contains( "Projet informatique" ) || e.Code == "IT-PIN-45-2"))
                    || (e.Classes.Any( c => (c & StudentClass.S05IL) == StudentClass.S05IL ) && (e.Title.Contains( "Projet informatique" ) || e.Code == "IT-PIN-45-2"));

			//Faire pareil pour TOUS les prof IL et SR
        }


        [Test]
        public void display_spi_calendar()
        {
            CalendarManager m = new CalendarManager( TestHelper.CacheFolder );
            m.Load( TestHelper.ConsoleMonitor);
			var all = m.Planning.Events.Where(SpiFilter).OrderBy(e => e.Beg);

            Console.WriteLine( "+++++++++++++++++++++++++++++++++++++++++" );
            foreach( var e in all )
            {
                Console.WriteLine( "----------" );
                Console.WriteLine( e );
            }
            Console.WriteLine( "+++++++++++++++++++++++++++++++++++++++++" );
        }

		[TestCase("S05")]
		[TestCase("S04-IL")]
		[TestCase("S03-SR")]
		[Test]
		public void display_semester_calendar(string test)
		{
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
			m.Load(TestHelper.ConsoleMonitor, test);
			var all = m.Planning.Events;

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in all)
			{
				Console.WriteLine("----------");
				Console.WriteLine(e);
			}
			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
		}

		[Test]
		public void testForThib()
		{
			//On met le path à la place de TestHepler.CacheFolder
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);

			//lit/creer le calendrier des S05

			//Dans le string peut etre un nom de prof et ressort le planning du prof

			m.Load(TestHelper.ConsoleMonitor, "S05");

			//prend le planning des IL
			var planningIL = m.Planning.EventsIL;

			//prend le planning des SR
			var planningSR = m.Planning.EventsSR;

			//prend le planning trier par date
			var planningDATE = m.Planning.EventsByDate;

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in planningDATE)
			{
				Console.WriteLine("----------");
				Console.WriteLine("Début : " + e.BegToString);
				Console.WriteLine("Durée : " + e.LenghtToString);
				Console.WriteLine("Fin : " + e.EndToString);
				Console.WriteLine("Titre : " + e.Title);
				Console.WriteLine("Code : " + e.Code);
				Console.WriteLine("Classes : " + e.ClassesToString);
				Console.WriteLine("Salle : " + e.Location);
				Console.WriteLine("Prof : " + e.TeachersToString);
			}
			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
		}
        [Test]
        public void update_spi_calendar()
        {
            CalendarManager m = new CalendarManager( TestHelper.CacheFolder );
            m.Load( TestHelper.ConsoleMonitor);
            var path = Path.Combine( TestHelper.CacheFolder, "Intecth-Calendar.ics" );
			File.WriteAllText(path, m.Planning.Events.ToiCalendar().ToCalendarString());
			//File.WriteAllText( path, m.Planning.Events.Where( SpiFilter ).ToiCalendar().ToCalendarString() );
        }

    }
}
