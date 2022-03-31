using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
    public class TVShow
    {
		public string URL { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Language { get; set; }
		public string Status { get; set; }
		public int Runtime { get; set; }
		public int AverageRuntime { get; set; }
		public DateTime Premiered { get; set; }
		public DateTime Ended { get; set; }
		public string OfficialSite { get; set; }
		public float Rating { get; set; }
		public TimeSpan ScheduleTime { get; set; }
		public string Image { get; set; }
		public string Summary { get; set; }
		public bool ScheduleMon { get; set; }
		public bool ScheduleTue { get; set; }
		public bool ScheduleWed { get; set; }
		public bool ScheduleThu { get; set; }
		public bool ScheduleFri { get; set; }
		public bool ScheduleSat { get; set; }
		public bool ScheduleSun { get; set; }
	}
}
