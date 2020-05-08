using LetterBuilderWebAdmin.Models;
using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Services.DAO
{
    public interface ICatalogDataAccess
    {
        void Add(Catalog entity);
        void UpdateName(Catalog entity);
        void UpdateOrder(Catalog entity);
        void UpdateParentCatalog(Catalog entity);
        void Delete(int id);
        List<Catalog> GetSubcatalogsByParentCatalogId(int parentCatalogId);
        List<Catalog> GetAll();
        Catalog GetById(int id);
    }
}
