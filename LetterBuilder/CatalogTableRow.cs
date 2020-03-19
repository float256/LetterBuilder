using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class CatalogTableRow
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int IDParentCatalog { get; set; }

        public CatalogTableRow(int id = 0, string name = "", int idParentCatalog = 0)
        {
            ID = id;
            Name = name;
            IDParentCatalog = idParentCatalog;
        }
    }
}
