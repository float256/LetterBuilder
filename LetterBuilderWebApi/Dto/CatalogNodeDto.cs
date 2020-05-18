using LetterBuilderCore.Models;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace LetterBuilderWebApi.Dto
{
    public class CatalogNodeDto : ICatalogNode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public List<ICatalogNode> ChildrenNodes { get; set; }
        public List<TextBlock> CatalogAttachments { get; set; }

        [JsonIgnore]
        public ICatalogNode ParentCatalog { get; set; }
        
        public CatalogNodeDto()
        {
            ChildrenNodes = new List<ICatalogNode>();
            CatalogAttachments = new List<TextBlock>();
        }
    }
}
