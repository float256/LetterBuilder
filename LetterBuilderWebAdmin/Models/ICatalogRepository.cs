using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Models
{
    public interface ICatalogRepository
    {
        void Add(Catalog entity);
        void Delete(int id);
        List<Catalog> GetAll();
        Catalog GetById(int id);
        List<Catalog> GetSubcatalogsByParentCatalogId(int parentCatalogId);
        void Update(Catalog entity);
    }
}