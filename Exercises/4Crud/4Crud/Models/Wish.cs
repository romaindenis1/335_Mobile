using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Crud.Models
{
    public class Wish
    {
        public int Id { get; set; }
        public string? Definition { get; set; }
        public DateTime AccomplishedDate { get; set; }

        public override string ToString()
        {
            return $"[Wish] {Id}";
        }
    }
}
