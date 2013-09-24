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
        Console.Write(ser.SerializeToString(output));

    }
}
