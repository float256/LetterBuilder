using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public interface ICatalogWriteDataAccess
    {
        void Add(Catalog entity);
        void UpdateName(Catalog entity);
        void UpdateOrder(Catalog entity);
        void UpdateParentCatalog(Catalog entity);
        void Delete(int id);
    }
}
