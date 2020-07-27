using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services
{
    public interface IDirectorySystemWriteFacade
    {
        void Add(Catalog catalog);
        void Add(TextBlock catalog);
        void Add(Picture picture);
        void DeleteTextBlock(int id);
        void DeleteCatalog(int id);
        void DeletePicture(int id);
        void UpdateValue(TextBlock textBlock);
        void UpdateValue(Catalog catalog);
        void UpdateOrder(TextBlock textBlock, OrderAction orderAction);
        void UpdateOrder(Catalog catalog, OrderAction orderAction);
        void UpdateParentCatalog(TextBlock textBlock);
        void UpdateParentCatalog(Catalog catalog);
    }
}
