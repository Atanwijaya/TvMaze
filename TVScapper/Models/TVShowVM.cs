using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
    public class TVShowVM
    {
		public int ID { get; set; }
		public string URL { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Language { get; set; }
		public string Status { get; set; }
		public int? Runtime { get; set; }
		public int? AverageRuntime { get; set; }
		public string Premiered { get; set; }
		public string Ended { get; set; }
		public string OfficialSite { get; set; }
		public float? Rating { get; set; }
		public string Image { get; set; }
		public string Summary { get; set; }
		public TVShowSchedules Schedules { get; set; }
        public List<PersonCharacter> Characters { get; set; }
        public List<string> Genres { get; set; }
    }
}
