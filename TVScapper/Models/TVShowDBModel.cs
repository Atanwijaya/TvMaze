using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
	public class TVShowDBModel
	{
		public int ID { get; set; }
		public string URL { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public string Language { get; set; }
		public string Status { get; set; }
		public int? Runtime { get; set; }
		public int? AverageRuntime { get; set; }
		public DateTime? Premiered { get; set; }
		public DateTime? Ended { get; set; }
		public string OfficialSite { get; set; }
		public float? Rating { get; set; }
		public string ScheduleTime { get; set; }
		public string Image { get; set; }
		public string Summary { get; set; }
		public bool ScheduleMon { get; set; }
		public bool ScheduleTue { get; set; }
		public bool ScheduleWed { get; set; }
		public bool ScheduleThu { get; set; }
		public bool ScheduleFri { get; set; }
		public bool ScheduleSat { get; set; }
		public bool ScheduleSun { get; set; }
		public int CastID { get; set; }
		public string CastURL { get; set; }
		public string CastName { get; set; }
		public string CastCountryName { get; set; }
		public string CastCountryCode { get; set; }
		public string CastCountryTZ { get; set; }
		public DateTime? CastBirthday { get; set; }
		public DateTime? CastDeathday { get; set; }
		public Char? CastGender { get; set; }
		public string CastImage { get; set; }
		public int CharacterID { get; set; }
		public string CharacterName { get; set; }
		public string CharacterURL { get; set; }
		public string CharacterIMG { get; set; }
		public string Genre { get; set; }
	}
}
