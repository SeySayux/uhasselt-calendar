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

    static readonly String[] kCalendars = new String[] {
        "677_2013_2014", // 3ba informatica (S) (1816, 1588, 2941, 2168, 1303, 2297)
        "678_2013_2014", // 3ba wiskunde (S) (1602)
        "628_2013_2014", // 1ba tew: hi (T) (1537)
        "666_2013_2014", // 1ba wiskunde (T) (0173)
        //"665_2013_2014", // 1ba fysica (T) (1824, 0173)
    };

    static readonly String[] kCourseIds = new String[] {
        "1537", // Natuurkunde en technologie
        "1824", // Optica
        "0173", // Mechanica
        "1816", // Computernetwerken
        "1588", // Wetenschapsfilosofie
        "2941", // Kansrekening en statistiek
        "2168", // Multimediatechnologie
        "1602", // Logica en modeltheorie
        "1303", // Software engineering
        "2297", // Bachelorproef
    };

    static readonly String[] kCourseShortHands = new String[] {
        "NATEC", // Natuurkunde en technologie
        "???",   // Optica
        "MECH",  // Mechanica
        "COMNE", // Computernetwerken
        "MOCEB", // Wetenschapsfilosofie
        "KSTAT", // Kansrekening en statistiek
        "MMT",   // Multimediatechnologie
        "LMT",   // Logica en modeltheorie
        "???",   // Software engineering
        "???",   // Bachelorproef
    };

    
    public static void Main(string[] args) {
        var serializer = new XmlSerializer(typeof(Schedule));

        var reader = new StreamReader(args[0] + ".xml");
        Schedule schedule = (Schedule)serializer.Deserialize(reader);

        var output = new iCalendar();

        foreach(var cal in schedule.Calendars) {
            var id = kCalendarUriPrefix + cal.Id + kCalendarUriSuffix;
            var cc = iCalendar.LoadFromUri(new Uri(id));
        
            foreach(var c in cc) {
                foreach(var e in c.Events) {
                    if(e.Description != null) {
                        // Filter on course number by default.
                        var match = Regex.Match(e.Description, @"\d\d\d\d");

                        if(!match.Success ||
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
        Console.WriteLine(ser.SerializeToString(output));

    }
}
