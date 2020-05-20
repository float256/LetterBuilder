using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Dto
{
    public class CatalogParserNodeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int ParentCatalogId { get; set; }
        public List<CatalogParserNodeDto> ChildrenNodes { get; set; }
        public List<TextBlock> CatalogAttachments { get; set; }

        public CatalogParserNodeDto()
        {
            ChildrenNodes = new List<CatalogParserNodeDto>();
            CatalogAttachments = new List<TextBlock>();
        }
    }
}
