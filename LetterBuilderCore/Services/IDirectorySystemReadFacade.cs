using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services
{
    public interface IDirectorySystemReadFacade
    {
        List<Catalog> GetAllCatalogs();
        List<TextBlock> GetAllTextBlocks();
        TextBlock GetTextBlockById(int id);
        Catalog GetCatalogById(int id);
        Picture GetPictureById(int id);
        List<Catalog> GetSubcatalogs(int id);
        List<TextBlock> GetCatalogAttachments(int id);
    }
}
