using System.Collections.Generic;


namespace LetterBuilderCore.Models
{
    public class CatalogNode : ICatalogNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<ICatalogNode> ChildrenNodes { get; set; }
        public List<TextBlock> CatalogAttachments { get; set; }
        public ICatalogNode ParentCatalog { get; set; }
        public CatalogNode()
        {
            ChildrenNodes = new List<ICatalogNode>();
            CatalogAttachments = new List<TextBlock>();
        }
    }
}