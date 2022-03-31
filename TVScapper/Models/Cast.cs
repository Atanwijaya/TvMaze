using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
    public class Cast
    {
        public string URL { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string CountryTZ { get; set; }
        public string Birthday { get; set; }
        public string Deathday { get; set; }
        public Char Gender { get; set; }
        public string Image { get; set; }
    }
}
