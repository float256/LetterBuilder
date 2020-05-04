using LetterBuilderWebAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Services
{
    public interface IDirectorySystemFacade
    {
        List<Catalog> GetAllCatalogs();
        List<TextBlock> GetAllTextBlocks();
        TextBlock GetTextBlockById(int id);
        Catalog GetCatalogById(int id);
        List<Catalog> GetSubcatalogs(int id);
        List<TextBlock> GetCatalogAttachments(int id);
    }
}
