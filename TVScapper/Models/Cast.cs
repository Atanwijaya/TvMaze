using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
    public class Cast
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryTZ { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? Deathday { get; set; }
        public Char? Gender { get; set; }
        public string Image { get; set; }
    }
}
