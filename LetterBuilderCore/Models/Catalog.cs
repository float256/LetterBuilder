using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderCore.Models
{
    public class Catalog
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCatalogId { get; set; }
        public int OrderInParentCatalog { get; set; }
    }
}
