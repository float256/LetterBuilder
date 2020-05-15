using LetterBuilderCore.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilderCore.Services.DAO
{
    public interface ICatalogReadDataAccess
    {
        List<Catalog> GetSubcatalogsByParentCatalogId(int parentCatalogId);
        List<Catalog> GetAll();
        Catalog GetById(int id);
    }
}
