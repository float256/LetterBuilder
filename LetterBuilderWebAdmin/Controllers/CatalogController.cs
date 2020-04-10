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
        public CatalogController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            CatalogRepository catalogRepository = new CatalogRepository(_connectionString);
            
            return View(catalogRepository.GetCatalogContent(id));
        }

        [HttpGet]
        public IActionResult Add(int id) => View();

        [HttpGet]
        public IActionResult Delete(int id) => View();

        [HttpGet]
        public IActionResult Update(int id) => View();

        [HttpPost]
        public IActionResult AddCatalog(Catalog catalog)
        {
            CatalogRepository catalogRepository = new CatalogRepository(_connectionString);
            catalogRepository.Add(catalog);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteCatalog(Catalog catalog)
        {
            CatalogRepository catalogRepository = new CatalogRepository(_connectionString);
            catalogRepository.Delete(catalog.Id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCatalog(Catalog catalog)
        {
            CatalogRepository catalogRepository = new CatalogRepository(_connectionString);
            catalogRepository.Update(catalog);
            return RedirectToAction("Index");
        }
    }
}