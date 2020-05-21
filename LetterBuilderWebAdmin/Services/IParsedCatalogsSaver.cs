using LetterBuilderWebAdmin.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Services
{
    public interface IParsedCatalogsSaver
    {
        void AddCatalogTree(CatalogParserNodeDto catalogNode);
    }
}
