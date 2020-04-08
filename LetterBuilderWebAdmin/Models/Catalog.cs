using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class Catalog : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCatalogId { get; set; }
    }
}
