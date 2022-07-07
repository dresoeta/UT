using System;
using System.Collections.Generic;

namespace UT.Dto
{
    public class PostUserDto
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string DateOfBirth { get; set; }
        public string Location { get; set; }
    }

    public class Timezone
    {
        public List<string> timeZone { get; set; }
    }

    public class Offset
    {
        public string abbreviation { get; set; }
        public string client_ip { get; set; }
        public DateTime datetime { get; set; }
        public int day_of_week { get; set; }
        public int day_of_year { get; set; }
        public bool dst { get; set; }
        public object dst_from { get; set; }
        public int dst_offset { get; set; }
        public object dst_until { get; set; }
        public int raw_offset { get; set; }
        public string timezone { get; set; }
        public int unixtime { get; set; }
        public DateTime utc_datetime { get; set; }
        public string utc_offset { get; set; }
        public int week_number { get; set; }
    }
}
