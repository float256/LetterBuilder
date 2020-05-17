using LetterBuilderWebAdmin.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Dto
{
    public class CatalogContent
    {
        public List<CatalogWithFieldVerifying> Catalogs { get; set; }
        public List<TextBlockWithFieldVerifying> TextBlocks { get; set; }
    }
}
