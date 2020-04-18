using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class OrderOfCatalog
    {
        public Catalog Catalog { get; set; }
        public int DepthLevel { get; set; }
    }
}
