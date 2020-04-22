using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class CatalogNode
    {
        public List<CatalogNode> ChildrenNodes { get; set; } = new List<CatalogNode>();
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsOpened { get; set; } = false;
        public CatalogNode ParentCatalog { get; set; }
        public int Order { get; set; }
    }
}
