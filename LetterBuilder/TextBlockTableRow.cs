using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class TextBlockTableRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int ParentCatalogId { get; set; }

        public TextBlockTableRow(int id = 0, string name = "", string text = "", int parentCatalogId = 0)
        {
            Id = id;
            Name = name;
            Text = text;
            ParentCatalogId = parentCatalogId;
        }
    }
}
