using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class CatalogContent
    {
        public List<Catalog> Catalogs { get; set; }
        public List<TextBlock> TextBlocks { get; set; }
    }
}
