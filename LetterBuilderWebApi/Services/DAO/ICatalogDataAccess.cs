using LetterBuilderWebAdmin.Models;
using System.Collections.Generic;

namespace LetterBuilderWebAdmin.Services.DAO
{
    public interface ICatalogDataAccess
    {
        List<Catalog> GetSubcatalogsByParentCatalogId(int parentCatalogId);
        List<Catalog> GetAll();
        Catalog GetById(int id);
    }
}
