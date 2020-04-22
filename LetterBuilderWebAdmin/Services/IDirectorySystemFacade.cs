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
        void Add(Catalog catalog);
        void Add(TextBlock catalog);
        void DeleteTextBlock(int id);
        void DeleteCatalog(int id);
        void UpdateValue(TextBlock textBlock);
        void UpdateValue(Catalog catalog);
        void UpdateOrder(TextBlock textBlock, OrderAction orderAction);
        void UpdateOrder(Catalog catalog, OrderAction orderAction);
    }
}
