using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class TextBlockTableRow
    {
        public int ID;
        public string Name;
        public string Text;
        public int IDParentCatalog;

        public TextBlockTableRow(int id = 0, string name = "", string text = "", int idParentCatalog = 0)
        {
            ID = id;
            Name = name;
            Text = text;
            IDParentCatalog = idParentCatalog;
        }
    }
}
