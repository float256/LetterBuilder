using LetterBuilderWebAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace LetterBuilderWebAdmin.ViewComponents
{
    public class DirectoryStructureViewComponent : ViewComponent
    {
        private ICatalogRepository _catalogRepository;

        public DirectoryStructureViewComponent(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }
        public IViewComponentResult Invoke(int id)
        {
            List<Catalog> allCatalogs = _catalogRepository.GetAll();
            List<CatalogDepthLevel> allOrderedCatalogs = new List<CatalogDepthLevel>();
            int currDepthLevel = 0;

            // Добавление каталогов, находящихся в корне файловой системы (такие элементы имеют ParentCatalogId = 0)
            foreach (var item in allCatalogs.FindAll(item => item.ParentCatalogId == 0))
            {
                allOrderedCatalogs.Add(new CatalogDepthLevel { Catalog = item, DepthLevel = 0 });
            }

            allCatalogs.RemoveAll(item => item.ParentCatalogId == 0);
            while (allCatalogs.Count > 0)
            {
                currDepthLevel++;
                
                // Получение индексов каталогов, распологающихся на 1 уровень выше текущих
                List<int> indicesOfParentCatalogs = new List<int>();
                foreach (CatalogDepthLevel parentCatalogOrder in allOrderedCatalogs.FindAll(item => item.DepthLevel == currDepthLevel - 1))
                {
                    indicesOfParentCatalogs.Add(parentCatalogOrder.Catalog.Id);
                }

                // Запись каталогов, находящихся на текущем уровне, в allOrderedCatalog и удаление их из allCatalogs
                for (int i = 0; i < allCatalogs.Count; i++)
                {
                    Catalog currCatalog = allCatalogs[i];
                    if (indicesOfParentCatalogs.Contains(currCatalog.ParentCatalogId))
                    {
                        allOrderedCatalogs.Add(new CatalogDepthLevel
                        {
                            Catalog = currCatalog,
                            DepthLevel = currDepthLevel
                        });
                        allCatalogs.Remove(currCatalog);
                        i--;
                    }
                }
            }
            return View(allOrderedCatalogs);
        }
    }
}
