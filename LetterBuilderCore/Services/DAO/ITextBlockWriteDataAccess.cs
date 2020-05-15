using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public interface ITextBlockWriteDataAccess
    {
        void Add(TextBlock entity);
        void Delete(int id);
        void UpdateNameAndText(TextBlock entity);
        void UpdateParentCatalog(TextBlock entity);
        void UpdateOrder(TextBlock entity);
    }
}
