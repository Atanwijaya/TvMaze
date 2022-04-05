using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TVScapper.Models
{
    public class TVShowPage
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public bool HasMore { get; set; }
        public List<TVShowVM> Items { get; set; }
    }
}
