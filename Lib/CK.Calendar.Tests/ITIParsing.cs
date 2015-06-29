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


			var events = m.Planning.Events;

			string getCode = String.Empty;

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in events)
			{
				Console.WriteLine("----------");
				Console.WriteLine("Début : " + e.BegToString);
				Console.WriteLine("Durée : " + e.LenghtToString);
				Console.WriteLine("Fin : " + e.EndToString);
				Console.WriteLine("Titre : " + e.Title);
				Console.WriteLine("Salle : " + e.Location);
				Console.WriteLine("Organizer : " + e.TeachersToString);
				Console.WriteLine("Code : " + e.Code);
				
			}
			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
		}

		[Test]
		[TestCase("Perso-54FEDGkDL")]
		[TestCase("EventITI")]
		public void RemoveData(string calendarName)
		{
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
			m.Load(TestHelper.ConsoleMonitor, calendarName);
			var events = m.Planning.Events;
			

			//string getCode = String.Empty;

			var ie = events.Where(x => x.Title == "Titre1").FirstOrDefault();
			if (ie != null) m.RemoveData(ie);

			var ie2 = events.Where(x => x.Title == "Titre5").FirstOrDefault();
			if (ie2 != null) m.RemoveData(ie.Code); ;

			m.SaveData();
			/*foreach (var e in events)
			{
				if (e.Title == "Titre1") m.RemoveData(e);
				else if (e.Title == "Titre5") getCode = e.Code;
			}*/

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in events)
			{
				Console.WriteLine("----------");
				Console.WriteLine("Début : " + e.BegToString);
				Console.WriteLine("Durée : " + e.LenghtToString);
				Console.WriteLine("Fin : " + e.EndToString);
				Console.WriteLine("Titre : " + e.Title);
				Console.WriteLine("Salle : " + e.Location);
				Console.WriteLine("Organizer : " + e.TeachersToString);
				Console.WriteLine("Code : " + e.Code);

			}
			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");

		}


		[Test]
		[TestCase("Perso-54FEDGkDL")]
		[TestCase("EventITI")]
		public void UpdateData(string calendarName)
		{
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);
			m.Load(TestHelper.ConsoleMonitor, calendarName);
			var events = m.Planning.Events;

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");


			//m.UpDateData();
		}

		[Test]
		[TestCase("Perso-54FEDGkDL")]
		[TestCase("EventITI")]
		public void AllInData(string calendarName)
		{
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);

			m.Load(TestHelper.ConsoleMonitor, calendarName, true);

			Assert.That(m.Planning != null);
			Assert.That(m.Planning.Events.Count() == 0);

			string[] organizer = { "Organizer1", "Organizer7" };
			string[] organizer2 = { "Organizer2", "Organizer6" };

			DateTime beg = new DateTime(2015, 6, 15, 11, 39, 0, DateTimeKind.Local);
			DateTime beg2 = new DateTime(2015, 6, 1, 11, 39, 0, DateTimeKind.Local);
			DateTime end = new DateTime(2015, 6, 29, 22, 00, 0, DateTimeKind.Local);

			m.AddData("Titre0", organizer, "EO85", beg, end);
			m.AddData("Titre5", organizer, "EO85", beg, end);
			m.AddData("Titre8", organizer2, "EO657", beg2, end);
			m.AddData("Titre44", organizer2, "EO657", beg2, end);
			m.AddData("Titre44", organizer2, "EO657", beg2, end);

			//Assert.That(m.Planning.Events.Count() == 4);

			var events = m.Planning.Events;

			var ie = events.Where(x => x.Title == "Titre0").FirstOrDefault();
			if (ie != null) m.RemoveData(ie);

			//Assert.That(m.Planning.Events.Count() == 3);

			var ie2 = events.Where(x => x.Title == "Titre8").FirstOrDefault();
			if (ie2 != null) m.RemoveData(ie2.Code);

			//Assert.That(m.Planning.Events.Count() == 2);

			m.SaveData();

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in events)
			{
				Console.WriteLine("----------");
				Console.WriteLine("Début : " + e.BegToString);
				Console.WriteLine("Durée : " + e.LenghtToString);
				Console.WriteLine("Fin : " + e.EndToString);
				Console.WriteLine("Titre : " + e.Title);
				Console.WriteLine("Salle : " + e.Location);
				Console.WriteLine("Organizer : " + e.TeachersToString);
				Console.WriteLine("Code : " + e.Code);

				//Bouton Supp
				if ( e.Title == "Titre5") m.RemoveData(e);
			}

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");

			//Assert.That(m.Planning.Events.Count() == 1);

		}
	}


}
