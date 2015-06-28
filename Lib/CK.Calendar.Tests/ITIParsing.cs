using CK.Calendar.Intech;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CK.Calendar.Tests
{  
	[TestFixture]
	class ITIParsing
	{
		[Test]
		[TestCase("Perso-54FEDGkDL")]
		[TestCase("EventITI")]
		[Explicit]
		public void add_event_all(string calendarName)
		{
			{
				CalendarManager m = new CalendarManager(TestHelper.CacheFolder);

				Assert.That(m.Planning == null);

				m.Load(TestHelper.ConsoleMonitor, calendarName, true);

				string [] organizer = { "Orga1", "Orga2"};

				//Ajoute Time : Année,Mois,Jour,Heure,Minute,Secondes,UTC
				DateTime beg = new DateTime(2015,6,28,21,39,0,DateTimeKind.Local);
				DateTime end = new DateTime(2015,6,28,22,00,0,DateTimeKind.Local);

				//Ajoute Un evenement : titre,organiseur,salle|endroit,debut,fin
				m.AddData("Titre1",organizer,"EOS",beg,end);

				//Sauvergarde du calendrier
				m.SaveData();

				Assert.That(m.Planning != null);
			}
			{
				CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
				m.Load(TestHelper.ConsoleMonitor, calendarName);
				string[] organizer = { "Orga4", "Orga3" };
				DateTime beg = new DateTime(2015, 6, 30, 21, 39, 0, DateTimeKind.Local);
				DateTime end = new DateTime(2015, 6, 30, 22, 00, 0, DateTimeKind.Local);
				m.AddData("Titre2", organizer, "EO9", beg, end);
				m.SaveData();
				Assert.That(m.Planning != null);
			}
		}

		[Test]
		[TestCase("Perso-54FEDGkDL")]
		[TestCase("EventITI")]
		[Explicit]
		public void add_event(string calendarName)
		{
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
			m.Load(TestHelper.ConsoleMonitor, calendarName);
			string[] organizer = { "Orga7", "Orga8" };
			DateTime beg = new DateTime(2015, 6, 30, 11, 39, 0, DateTimeKind.Local);
			DateTime end = new DateTime(2015, 6, 29, 22, 00, 0, DateTimeKind.Local);
			m.AddData("Titre1", organizer, "EO5", beg, end);
			m.AddData("Titre5", organizer, "EO7", beg, end);
			m.SaveData();
			Assert.That(m.Planning != null);
			
		}

		[Test]
		[TestCase("Perso-54FEDGkDL")]
		[TestCase("EventITI")]
		public void read_calendar(string calendarName)
		{
			//On met le path à la place de TestHepler.CacheFolder
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);


			m.Load(TestHelper.ConsoleMonitor, calendarName);


			//prend le planning trier par date
			var planning = m.Planning.Events;

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in planning)
			{
				Console.WriteLine("----------");
				Console.WriteLine("Début : " + e.BegToString);
				Console.WriteLine("Durée : " + e.LenghtToString);
				Console.WriteLine("Fin : " + e.EndToString);
				Console.WriteLine("Titre : " + e.Title);
				Console.WriteLine("Salle : " + e.Location);
				Console.WriteLine("Organizer : " + e.TeachersToString);
			}
			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
		}


	}
}
