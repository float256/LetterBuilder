using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderWebAdmin.Models;
using LetterBuilderWebAdmin.Services;

namespace LetterBuilderWebAdmin.Controllers
{
    public class CatalogController : Controller
    {
        private IDirectorySystemFacade _directorySystemFacade;

        public CatalogController(IDirectorySystemFacade directorySystemFacade)
        {
            _directorySystemFacade = directorySystemFacade;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            CatalogContent catalogContent = new CatalogContent
            {
                Catalogs = _directorySystemFacade.GetSubcatalogs(id),
                TextBlocks = _directorySystemFacade.GetCatalogAttachments(id)
            };
            return View(catalogContent);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            return View(_directorySystemFacade.GetCatalogById(id));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_directorySystemFacade.GetCatalogById(id));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_directorySystemFacade.GetCatalogById(id));
        }

        [HttpPost]
        public IActionResult AddCatalog(Catalog catalog)
        {
            _directorySystemFacade.Add(catalog);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteCatalog(int id)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            _directorySystemFacade.DeleteCatalog(catalog.Id);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateCatalog(Catalog catalog)
        {
            _directorySystemFacade.UpdateValue(catalog);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult Move(int id, OrderAction action)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            _directorySystemFacade.UpdateOrder(catalog, action);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }
    }
}