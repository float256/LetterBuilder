using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Models
{
    public interface ITextBlockRepository
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
