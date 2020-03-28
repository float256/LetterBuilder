using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class TextBlocksReadingResult
    {
        public Dictionary<int, TextBlockTableRow> TextBlockTableInfo;
        public Dictionary<int, List<int>> CatalogToChildTextBlocksMap;
    }
}
