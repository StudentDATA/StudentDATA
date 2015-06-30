using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDay.iCal;
using DDay.iCal.Serialization;
using DDay.iCal.Serialization.iCalendar;

namespace CK.Calendar.Intech
{
    public static class SchoolEventExtension
    {

        public static iCalendar ToiCalendar( this IEnumerable<SchoolEvent> @this )
        {
            iCalendar iCal = new DDay.iCal.iCalendar();
			iCal.Version = "1.0";
			iCal.Name = "Student DATA Calendar";
			iCal.ProductID = "Calendar Manager SD & Olivier Spinelli";

			//Refaire personna
            foreach( var e in @this )
            {
                Event evt = iCal.Create<Event>();
                evt.Start = new iCalDateTime( e.Beg );
                evt.End = new iCalDateTime( e.End );

				//Refaire l'organiseur avec le parse pour plusieurs
				evt.Organizer = new Organizer(e.Organizer.Values.FirstOrDefault());
				evt.Organizer.CommonName = e.Organizer.Keys.FirstOrDefault();

				//Pour rajouter Nom du ficher : retouver la nom de la prioprieté
				evt.AddProperty("", "");

                evt.Location = e.Location;

				//Refaire pour l'ITIEVENT
                evt.Summary = String.Join( ", ", e.Classes.Select( c => c.ToExplicitString() ) ) + " / " + String.Join( ", ", e.Teachers );
                evt.Description = e.Code + " / " + e.Title;
            }
            return iCal;
        }

        public static string ToCalendarString( this iCalendar @this )
        {
            ISerializationContext ctx = new SerializationContext();
            ISerializerFactory factory = new SerializerFactory();
            IStringSerializer serializer = factory.Build( @this.GetType(), ctx ) as IStringSerializer;
            return serializer.SerializeToString( @this );
        }

    }
}
