using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class CatalogNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<CatalogNode> ChildrenNodes { get; set; }
        public List<TextBlock> CatalogAttachments { get; set; }
        public CatalogNode()
        {
            ChildrenNodes = new List<CatalogNode>();
            CatalogAttachments = new List<TextBlock>();
        }
    }
}
