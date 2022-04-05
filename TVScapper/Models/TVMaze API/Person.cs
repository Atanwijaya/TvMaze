using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models.TVMaze_API
{
    public class Person
    {
        public int ID { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTime? Deathday { get; set; }
        public string Gender { get; set; }
        public Image Image { get; set; }
    }
}
