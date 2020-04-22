using LetterBuilderWebAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using LetterBuilderWebAdmin.Services.DAO;

namespace LetterBuilderWebAdmin.ViewComponents
{
    public class DirectoryStructureViewComponent : ViewComponent
    {
        private ICatalogDataAccess _catalogRepository;

        public DirectoryStructureViewComponent(ICatalogDataAccess catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }
        public IViewComponentResult Invoke(int id)
        {
            List<Catalog> allCatalogs = _catalogRepository.GetAll();
            List<CatalogNode> topLevelCatalogNodes = new List<CatalogNode>();
            List<CatalogNode> catalogsOnCurrDepthLevel = new List<CatalogNode>();

            // Добавление каталогов, находящихся в корне файловой системы (такие элементы имеют ParentCatalogId = 0)
            foreach (Catalog item in allCatalogs.FindAll(item => item.ParentCatalogId == 0))
            {
                topLevelCatalogNodes.Add(new CatalogNode
                {
                    Id = item.Id,
                    Name = item.Name,
                    IsSelected = (item.Id == id),
                    IsOpened = (item.Id == id),
                    Order = item.OrderInParentCatalog
                });
            }
            topLevelCatalogNodes.Sort((a, b) => a.Order.CompareTo(b.Order));

            // Построение дерева каталогов
            catalogsOnCurrDepthLevel = topLevelCatalogNodes.ToList();
            while (catalogsOnCurrDepthLevel.Count > 0)
            {
                List<CatalogNode> catalogsOnNextDepthLevel = new List<CatalogNode>();
                foreach (CatalogNode currParentCatalog in catalogsOnCurrDepthLevel)
                {
                    List<Catalog> allSubcatalogs = allCatalogs.FindAll(item => item.ParentCatalogId == currParentCatalog.Id);
                    allSubcatalogs.Sort((a, b) => a.OrderInParentCatalog.CompareTo(b.OrderInParentCatalog));
                    foreach (Catalog currSubcatalog in allSubcatalogs)
                    { 
                        CatalogNode node = new CatalogNode
                        {
                            Id = currSubcatalog.Id,
                            Name = currSubcatalog.Name,
                            ParentCatalog = currParentCatalog,
                            Order = currSubcatalog.OrderInParentCatalog
                        };
                        currParentCatalog.ChildrenNodes.Add(node);
                        catalogsOnNextDepthLevel.Add(node);

                        // Выставление флагов IsSelected и IsOpened в true у родительских каталогов, если
                        // текущий рассматриваемый каталог является
                        // каталогом, в котором в данный момент находится пользователь
                        if (node.Id == id)
                        {
                            node.IsOpened = node.IsSelected = true;
                            CatalogNode parentNode = node.ParentCatalog;
                            while (parentNode != null)
                            {
                                parentNode.IsOpened = true;
                                parentNode = parentNode.ParentCatalog;
                            }
                        }
                    }
                }
                catalogsOnCurrDepthLevel = catalogsOnNextDepthLevel;
            }
            return View(topLevelCatalogNodes);
        }
    }
}
