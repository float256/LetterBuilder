using LetterBuilderWebAdmin.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Dto
{
    public class CatalogContent
    {
        public List<CatalogDto> Catalogs { get; set; }
        public List<TextBlockDto> TextBlocks { get; set; }
    }
}
