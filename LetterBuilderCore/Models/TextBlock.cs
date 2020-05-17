using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderCore.Models
{
    public class TextBlock
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public int ParentCatalogId { get; set; }

        public int OrderInParentCatalog { get; set; }
    }
}

