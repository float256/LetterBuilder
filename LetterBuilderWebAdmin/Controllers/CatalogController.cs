using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderWebAdmin.Models;

namespace LetterBuilderWebAdmin.Controllers
{
    public class CatalogController : Controller
    {
        private string _connectionString;
        private CatalogRepository _catalogRepository;
        private TextBlockRepository _textBlockRepository;

        public CatalogController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
            _catalogRepository = new CatalogRepository(_connectionString);
            _textBlockRepository = new TextBlockRepository(_connectionString);
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            CatalogContent catalogContent = new CatalogContent
            {
                Catalogs = _catalogRepository.GetSubcatalogsByParentCatalogId(id),
                TextBlocks = _textBlockRepository.GetTextBlocksByParentCatalogId(id)
            };
            return View(catalogContent);
        }

        [HttpGet]
        public IActionResult Add(int id) => View(_catalogRepository.GetById(id));

        [HttpGet]
        public IActionResult Delete(int id) => View(_catalogRepository.GetById(id));

        [HttpGet]
        public IActionResult Update(int id) => View(_catalogRepository.GetById(id));

        [HttpPost]
        public IActionResult AddCatalog(Catalog catalog)
        {
            _catalogRepository.Add(catalog);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteCatalog(int id)
        {
            Catalog catalog = _catalogRepository.GetById(id);
            _catalogRepository.Delete(catalog.Id);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateCatalog(Catalog catalog)
        {
            _catalogRepository.Update(catalog);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }
    }
}