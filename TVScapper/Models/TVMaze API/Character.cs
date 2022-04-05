using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models.TVMaze_API
{
    public class Character
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public Image Image { get; set; }
    }
}
