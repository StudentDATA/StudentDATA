using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;
using NUnit.Framework;

namespace CK.Calendar.Tests
{
    [TestFixture]
    public class BasicICalTests
    {
        [Test]
        public void get_from_urls()
        {
			var all = iCalendar.LoadFromUri(new Uri(@"https://edt.esiea.fr/Telechargements/ical/EdT_INTECH_S05___2015M.ics?version=13.0.2.0&idICal=2B6BF0FA8E30EE3A2E0C181F48808C11&param=643d5b312e2e36325d2666683d3126663d3131303030"));
			foreach( var c in all )
            {
				var eventCal = c.Events.OrderBy(x => x.Start.Value);
				foreach (var e in eventCal)
                {
                    Console.WriteLine( "---------" );
                    Console.Write( "->Description: {0}", e.Description );
                    Console.WriteLine();
                    Console.Write( "->Summary: {0}", e.Summary );
                    Console.WriteLine();
                    Console.Write( "->Date: {0} -> {1}", e.Start.Value, e.End.Value );
                    Console.WriteLine();
                    Console.Write( "->Location: {0}", e.Location );
                    Console.WriteLine();
                    Console.WriteLine( "---------" );
                    if( e.Description != null && e.Description.Contains( "SPINELLI" ) )
                    {
                        Console.WriteLine( "{0}", e.ToString() );
                    }
                }
            }

        }
    }
}
