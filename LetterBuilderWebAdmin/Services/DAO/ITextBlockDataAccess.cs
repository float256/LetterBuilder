using LetterBuilderWebAdmin.Models;
using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Services.DAO
{
    public interface ITextBlockDataAccess
    {
        void Add(TextBlock entity);
        void Delete(int id);
        List<TextBlock> GetAll();
        TextBlock GetById(int id);
        List<TextBlock> GetTextBlocksByParentCatalogId(int parentCatalogId);
        void UpdateNameAndText(TextBlock entity);
        void UpdateOrder(TextBlock entity);
    }
}
