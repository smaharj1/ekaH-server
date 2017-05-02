using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ekaH_server.Models
{
    /// <summary>
    /// This class holds the days start time and end time.
    /// </summary>
    public class DayInfo
    {
        /// <summary>
        /// It holds the day of the week.
        /// </summary>
        public DayOfWeek Day { get; set; }

        /// <summary>
        /// It holds the start time.
        /// </summary>
        public TimeSpan startTime { get; set; }

        /// <summary>
        /// It holds the end time.
        /// </summary>
        public TimeSpan endTime { get; set; }
    }
}