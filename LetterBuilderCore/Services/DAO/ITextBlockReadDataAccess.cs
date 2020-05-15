using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public interface ITextBlockReadDataAccess
    {
        List<TextBlock> GetAll();
        TextBlock GetById(int id);
        List<TextBlock> GetTextBlocksByParentCatalogId(int parentCatalogId);
    }
}
