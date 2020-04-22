using LetterBuilderWebAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using LetterBuilderWebAdmin.Services;

namespace LetterBuilderWebAdmin.ViewComponents
{
    public class DirectoryStructureViewComponent : ViewComponent
    {
        private IDirectorySystemFacade _directoryFacade;

        public DirectoryStructureViewComponent(IDirectorySystemFacade catalogDataAccess)
        {
            _directoryFacade = catalogDataAccess;
        }

        public IViewComponentResult Invoke(int id)
        {
            List<CatalogNode> topLevelCatalogNodes = new List<CatalogNode>();
            List<CatalogNode> catalogsOnCurrDepthLevel = new List<CatalogNode>();

            // Добавление каталогов, находящихся в корне файловой системы (такие элементы имеют ParentCatalogId = 0)
            foreach (Catalog item in _directoryFacade.GetSubcatalogs(0))
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

            // Построение дерева каталогов
            catalogsOnCurrDepthLevel = topLevelCatalogNodes.ToList();
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
