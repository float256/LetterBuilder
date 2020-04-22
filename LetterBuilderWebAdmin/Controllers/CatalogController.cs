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
        private IDirectorySystemFacade _fileSystemRepository;

        public CatalogController(IDirectorySystemFacade fileSystemRepository)
        {
            _fileSystemRepository = fileSystemRepository;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            CatalogContent catalogContent = new CatalogContent
            {
                Catalogs = _fileSystemRepository.GetSubcatalogs(id),
                TextBlocks = _fileSystemRepository.GetCatalogAttachments(id)
            };
            return View(catalogContent);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            return View(_fileSystemRepository.GetCatalogById(id));
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_fileSystemRepository.GetCatalogById(id));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_fileSystemRepository.GetCatalogById(id));
        }

        [HttpPost]
        public IActionResult AddCatalog(Catalog catalog)
        {
            _fileSystemRepository.Add(catalog);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteCatalog(int id)
        {
            Catalog catalog = _fileSystemRepository.GetCatalogById(id);
            _fileSystemRepository.DeleteCatalog(catalog.Id);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateCatalog(Catalog catalog)
        {
            _fileSystemRepository.UpdateValue(catalog);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        public IActionResult MoveUp(int id)
        {
            Catalog catalog = _fileSystemRepository.GetCatalogById(id);
            _fileSystemRepository.UpdateOrder(catalog, catalog.OrderInParentCatalog - 1);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        public IActionResult MoveDown(int id)
        {
            Catalog catalog = _fileSystemRepository.GetCatalogById(id);
            _fileSystemRepository.UpdateOrder(catalog, catalog.OrderInParentCatalog + 1);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }
    }
}