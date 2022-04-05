using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models.TVMaze_API
{
    public class Schedule
    {
        public string Time { get; set; }
        public List<string> Days { get; set; }
    }
}
