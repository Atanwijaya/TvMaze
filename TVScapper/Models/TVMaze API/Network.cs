using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models.TVMaze_API
{
    public class Network
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public string OfficialSite { get; set; }
    }
}
