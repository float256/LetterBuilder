using LetterBuilderWebAdmin.Services;
using LetterBuilderWebAdmin.Services.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Models
{
    public class CatalogsTreeBuilder
    {
        private IDirectorySystemFacade _directoryFacade;
        public CatalogsTreeBuilder(IDirectorySystemFacade directorySystemFacade)
        {
            _directoryFacade = directorySystemFacade;
        }


        /// <summary>
        /// Данный метод строит дерево каталогов
        /// </summary>
        /// <param name="id">Id каталога, с которого нужно начинать строить дерево</param>
        /// <returns>Объект типа CatalogNode, который является начальным каталогом</returns>
        public CatalogNode BuildTree(int id)
        {
            Catalog initalCatalog = _directoryFacade.GetCatalogById(id);
            CatalogNode result = new CatalogNode
            {
                Id = initalCatalog.Id,
                Name = initalCatalog.Name,
                CatalogAttachments = _directoryFacade.GetCatalogAttachments(id),
                Order = initalCatalog.OrderInParentCatalog
            };

            // Получение подкаталогов и файлов, находящихся в изначальном каталоге
            foreach (Catalog subcatalog in _directoryFacade.GetSubcatalogs(id))
            {
                CatalogNode subcatalogNode = new CatalogNode
                {
                    Id = subcatalog.Id,
                    Name = subcatalog.Name,
                    CatalogAttachments = _directoryFacade.GetCatalogAttachments(subcatalog.Id),
                    Order = subcatalog.OrderInParentCatalog
                };
                result.ChildrenNodes.Add(subcatalogNode);
            }

            // Построение дерева
            List<CatalogNode> catalogsOnCurrDepthLevel = result.ChildrenNodes.ToList();
            while (catalogsOnCurrDepthLevel.Count > 0)
            {
                List<CatalogNode> catalogsOnNextDepthLevel = new List<CatalogNode>();
                foreach (CatalogNode currParentCatalog in catalogsOnCurrDepthLevel)
                {
                    List<Catalog> allSubcatalogs = _directoryFacade.GetSubcatalogs(currParentCatalog.Id);
                    foreach (Catalog currSubcatalog in allSubcatalogs)
                    {
                        CatalogNode node = new CatalogNode
                        {
                            Id = currSubcatalog.Id,
                            Name = currSubcatalog.Name,
                            Order = currSubcatalog.OrderInParentCatalog,
                            CatalogAttachments = _directoryFacade.GetCatalogAttachments(currSubcatalog.Id)
                        };
                        currParentCatalog.ChildrenNodes.Add(node);
                        catalogsOnNextDepthLevel.Add(node);
                    }
                }
                catalogsOnCurrDepthLevel = catalogsOnNextDepthLevel;
            }
            return result;
        }
    }
}
