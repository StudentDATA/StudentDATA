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

				//string [] organizer = { "Orga1", "Orga2"};
				var organizer = new Dictionary<string,string>();
				organizer.Add("OrgaName","Orga@mail.com");

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
				var organizer = new Dictionary<string, string>();
				organizer.Add("OrgaName", "Orga@mail.com");
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
			var organizer = new Dictionary<string, string>();
			organizer.Add("OrgaName", "Orga@mail.com");
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
			if (ie2 != null) m.RemoveData(ie2.Code); ;

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

			var organizer = new Dictionary<string, string>();
			organizer.Add("OrgaName", "Orga@mail.com");

			DateTime beg = new DateTime(2016, 6, 15, 11, 39, 0, DateTimeKind.Local);
			DateTime end = new DateTime(2016, 6, 29, 22, 00, 0, DateTimeKind.Local);


			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in events)
			{
				if (e.Title == "Titre5") m.UpDateData(e, "TitreTest", "Os2", organizer, beg, end);
				if (e.Title == "Titre1") m.UpDateData(e, e.Title, e.Location, organizer, e.Beg, e.End);
				if (e.Title == "Titre2") e.Change("Titre0","84s",e.Teachers.ToArray(),e.Beg,e.End);

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
		public void AllInData(string calendarName)
		{
			CalendarManager m = new CalendarManager(TestHelper.CacheFolder);

			m.Load(TestHelper.ConsoleMonitor, calendarName, true);

			Assert.That(m.Planning != null);
			Assert.That(m.Planning.Events.Count() == 0);

			var organizer = new Dictionary<string, string>();
			organizer.Add("OrgaName", "Orga@mail.com");
			var organizer2 = new Dictionary<string, string>();
			organizer.Add("OrgaName2", "Orga2@mail.com");

			DateTime beg = new DateTime(2015, 6, 15, 11, 39, 0, DateTimeKind.Local);
			DateTime beg2 = new DateTime(2015, 6, 1, 11, 39, 0, DateTimeKind.Local);
			DateTime end = new DateTime(2015, 6, 29, 22, 00, 0, DateTimeKind.Local);

			m.AddData("Titre0", organizer, "EO85", beg, end);
			m.AddData("Titre5", organizer, "EO85", beg, end);
			m.AddData("Titre8", organizer2, "EO657", beg2, end);
			m.AddData("Titre44", organizer2, "EO657", beg2, end);

			Assert.That(m.Planning.Events.Count() == 4);

			var events = m.Planning.Events;

			var ie = events.Where(x => x.Title == "Titre0").FirstOrDefault();
			if (ie != null) m.RemoveData(ie);

			Assert.That(m.Planning.Events.Count() == 3);

			var ie2 = events.Where(x => x.Title == "Titre8").FirstOrDefault();
			if (ie2 != null) m.RemoveData(ie2.Code);

			Assert.That(m.Planning.Events.Count() == 2);

			m.SaveData();

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");
			foreach (var e in events)
			{
				if (e.Title == "Titre5") m.RemoveData(e);
				else
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
			}

			Console.WriteLine("+++++++++++++++++++++++++++++++++++++++++");

			Assert.That(m.Planning.Events.Count() == 1);

		}
	}


}
