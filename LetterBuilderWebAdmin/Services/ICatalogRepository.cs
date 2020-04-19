using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Models
{
    public interface ICatalogRepository
    {
        void Add(Catalog entity);
        void UpdateName(Catalog entity);
        void UpdateOrder(Catalog entity);
        void Delete(int id);
        List<Catalog> GetSubcatalogsByParentCatalogId(int parentCatalogId);
        List<Catalog> GetAll();
        Catalog GetById(int id);
    }
}
