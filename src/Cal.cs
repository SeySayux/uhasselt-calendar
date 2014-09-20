using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

public class Schedule {
    public Calendar[] Calendars { get; set; }
}

public class Calendar {
    [XmlAttribute("primary")]
    public bool Primary { get; set; }
    public String Id { get; set; }
    public Course[] Courses { get; set; }
}

public class Course {
    public String Id { get; set; }
    public String Abbreviation { get; set; }
}

class Cal {

    const string kCalendarUriPrefix = "http://collegeroosters.uhasselt.be/";

    const string kCalendarUriSuffix = ".ics";

    public static void InjectCustomEvents(Course course, iCalendar output) {
        switch(course.Id) {
        case "0173": {// mechanica
            Event e = new Event();
            e.Description = 
                    "MECH; G8; (Practicum); Voor: 3de bachelor informatica";
            e.Summary = e.Description;
            e.Location = "G8";
            e.Class = "PUBLIC";
            e.UID = "000000_injected";
            e.DTStart = new iCalDateTime(2013, 11, 08, 12, 00, 00);
            e.DTStart.IsUniversalTime = true;
            e.DTEnd = new iCalDateTime(2013, 11, 08, 16, 00, 00);
            e.DTEnd.IsUniversalTime = true;

            output.AddChild(e);
            break;
        }
        }
    }

    public static void Main(string[] args) {
        bool debug = args.Length == 2;

        var serializer = new XmlSerializer(typeof(Schedule));

        var reader = new StreamReader(args[0] + ".xml");
        Schedule schedule = (Schedule)serializer.Deserialize(reader);

        var output = new iCalendar();

        foreach(var cal in schedule.Calendars) {
            var id = kCalendarUriPrefix + cal.Id + kCalendarUriSuffix;
            var cc = iCalendar.LoadFromUri(new Uri(id));

            foreach(var course in cal.Courses)
                InjectCustomEvents(course, output);
        
            foreach(var c in cc) {
                foreach(var e in c.Events) {
                    // Append calendar Id to event.
                    if(e.Summary != null && debug) {
                        e.Summary = e.Summary + " [" + cal.Id + "]";
                    }
                    
                    if(e.Description != null) {
                        // Filter on course number by default.
                        var match = Regex.Match(e.Description, @"\d\d\d\d");

                        if((!match.Success && cal.Primary) ||
                                cal.Courses.Any(x => x.Id == match.Value)) {
                            output.AddChild(e.Copy<Event>());
                        }
                    } else {
                        // Filter on course shorthand when the university fucks
                        // up.
                        if(e.Summary == null) {
                            // This should not happen... ever.
                            continue;
                        }
                        
                        e.Description = e.Summary;

                        if(cal.Courses.Any(
                                x => e.Description.Contains(x.Abbreviation))) {
                            output.AddChild(e.Copy<Event>());
                        }
                    }
                }
            }
        }

        var ser = new iCalendarSerializer();
        Console.Write(ser.SerializeToString(output));

    }
}
