using LetterBuilderCore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using LetterBuilderCore.Models;
using LetterBuilderWebAdmin.Dto;

namespace LetterBuilderWebAdmin.ViewComponents
{
    public class DirectoryStructureViewComponent : ViewComponent
    {
        private IDirectorySystemFacade _directoryFacade;

        public DirectoryStructureViewComponent(IDirectorySystemFacade directoryFacade)
        {
            _directoryFacade = directoryFacade;
        }

        /// <summary>
        /// Данная функция отстраивает дерево каталогов
        /// </summary>
        /// <param name="id">Id каталога, в котором находится пользователь</param>
        /// <param name="structureType">Данная переменная определяет, какая разновидность дерева каталогов будет отображена</param>
        /// <returns></returns>
        public IViewComponentResult Invoke(int id, DirectoryStructureTypes structureType = DirectoryStructureTypes.MenuSidebar)
        {
            CatalogsTreeBuilderWithCollapsing treeBuilder = new CatalogsTreeBuilderWithCollapsing(_directoryFacade, id);
            if (structureType == DirectoryStructureTypes.TextBlockParentCatalogChangingMenu)
            {
                return View("TextBlockParentCatalogChangingMenu", treeBuilder.BuildTree().ChildrenNodes.Select(x => (CatalogNodeWithCollapsing)x).ToList());
            }
            else if (structureType == DirectoryStructureTypes.CatalogParentCatalogChangingMenu)
            {
                return View("CatalogParentCatalogChangingMenu", treeBuilder.BuildTree().ChildrenNodes.Select(x => (CatalogNodeWithCollapsing)x).ToList());
            }
            else
            {
                return View(treeBuilder.BuildTree().ChildrenNodes.Select(x => (CatalogNodeWithCollapsing)x).ToList());
            }
        }
    }
}
