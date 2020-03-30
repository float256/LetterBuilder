using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class CatalogsReadingResult
    {
        public Dictionary<int, CatalogTableRow> CatalogTableInfo;
        public Dictionary<int, List<int>> CatalogToChildCatalogsMap;
    }
}
