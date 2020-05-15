using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Dto
{
    public class CatalogNodeWithCollapsing : ICatalogNode, ICollapsedNode
    {
        public List<ICatalogNode> ChildrenNodes { get; set; } = new List<ICatalogNode>();
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
        public bool IsOpened { get; set; } = false;
        public ICatalogNode ParentCatalog { get; set; }
        public int Order { get; set; }
        public List<TextBlock> CatalogAttachments { get; set; }
    }
}
