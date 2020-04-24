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

        public IViewComponentResult Invoke(int id)
        {
            CatalogsTreeBuilder treeBuilder = new CatalogsTreeBuilder(_directoryFacade);
            return View(treeBuilder.BuildTree(id));
        }
    }
}
