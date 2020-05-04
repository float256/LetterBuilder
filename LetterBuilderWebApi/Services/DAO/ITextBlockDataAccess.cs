using LetterBuilderWebAdmin.Models;
using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Services.DAO
{
    public interface ITextBlockDataAccess
    {
        List<TextBlock> GetAll();
        TextBlock GetById(int id);
        List<TextBlock> GetTextBlocksByParentCatalogId(int parentCatalogId);
    }
}
