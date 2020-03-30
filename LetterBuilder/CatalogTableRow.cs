using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class CatalogTableRow
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentCatalogId { get; set; }

        public CatalogTableRow(int id = 0, string name = "", int parentCatalogId = 0)
        {
            Id = id;
            Name = name;
            ParentCatalogId = parentCatalogId;
        }
    }
}
