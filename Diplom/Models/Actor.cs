using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Diplom.Models
{
    public class Actor
    {
        public int id { get; set; }
        public string? Name { get; set; }
        public virtual List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
