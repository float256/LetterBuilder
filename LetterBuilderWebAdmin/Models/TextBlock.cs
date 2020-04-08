using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class TextBlock : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int ParentCatalogId { get; set; }

        public TextBlock(int id = 0, string name = "", string text = "", int parentCatalogId = 0)
        {
            Id = id;
            Name = name;
            Text = text;
            ParentCatalogId = parentCatalogId;
        }
    }

}

