using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Models
{
    public interface ICatalogNode
    {
        int Id { get; set; }
        string Name { get; set; }
        int Order { get; set; }
        List<ICatalogNode> ChildrenNodes { get; set; }
        List<TextBlock> CatalogAttachments { get; set; }
        ICatalogNode ParentCatalog { get; set; }
    }
}
