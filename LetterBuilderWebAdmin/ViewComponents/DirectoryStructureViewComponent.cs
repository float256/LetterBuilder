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

        public DirectoryStructureViewComponent(IDirectorySystemFacade directoryFacade)
        {
            _directoryFacade = directoryFacade;
        }

        /// <summary>
        /// Данная функция отстраивает дерево каталогов
        /// </summary>
        /// <param name="id">Id каталога, в котором находится пользователь</param>
        /// <param name="isMoveElementsInRadioButtons">
        /// Если true, то каждое имя каталога помещается в label.
        /// Также создается radio button с name='catalog-radio'
        /// </param>
        public IViewComponentResult Invoke(int id, DirectoryStructureTypes structureType = DirectoryStructureTypes.MenuSidebar)
        {
            CatalogsTreeBuilder treeBuilder = new CatalogsTreeBuilder(_directoryFacade);
            if (structureType == DirectoryStructureTypes.TextBlockParentCatalogChangingMenu)
            {
                return View("TextBlockParentCatalogChangingMenu", treeBuilder.BuildTree(id));
            }
            else if (structureType == DirectoryStructureTypes.CatalogParentCatalogChangingMenu)
            {
                return View("CatalogParentCatalogChangingMenu", treeBuilder.BuildTree(id));
            }
            else
            {
                return View(treeBuilder.BuildTree(id));
            }
        }
    }
}
