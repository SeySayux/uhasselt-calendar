using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DDay.iCal;
using DDay.iCal.Serialization.iCalendar;

class Cal {

    const string kCalendarUriPrefix = "http://collegeroosters.uhasselt.be/";

    const string kCalendarUriSuffix = ".ics";

    static readonly String[] kCalendars = new String[] {
        "677_2013_2014", // 3ba informatica (S) (1816, 1588, 2941, 2168, 1303, 2297)
        "678_2013_2014", // 3ba wiskunde (S) (1602)
        "628_2013_2014", // 1ba tew: hi (T) (1537)
        "665_2013_2014", // 1ba fysica (T) (1824, 0173)
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
        "2297", // Bacheloerproef
    };

    public static void Main() {
        var calendars = kCalendars
            .Select(x => new Uri(kCalendarUriPrefix + x + kCalendarUriSuffix))
            .Select(x => iCalendar.LoadFromUri(x));

        
        var output = new iCalendar();
        
        foreach(var cc in calendars) {
            foreach(var c in cc) {
                foreach(var e in c.Events) {
                    var match = Regex.Match(e.Description, @"\d\d\d\d");

                    if(!match.Success ||
                            kCourseIds.Any(x => x == match.Value)) {
                        output.AddChild(e.Copy<Event>());
                    }
                }
            }
        }

        var ser = new iCalendarSerializer();
        Console.WriteLine(ser.SerializeToString(output));

    }
}
