using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderCore.Services;
using LetterBuilderCore.Models;
using LetterBuilderWebAdmin.Dto;

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
                Catalogs = _directorySystemFacade.GetSubcatalogs(id).Select(
                    x => new CatalogWithFieldVerifying
                    {
                        Id = x.Id,
                        Name = x.Name,
                        OrderInParentCatalog = x.OrderInParentCatalog,
                        ParentCatalogId = x.ParentCatalogId
                    }).ToList(),
                TextBlocks = _directorySystemFacade.GetCatalogAttachments(id).Select(
                    x => new TextBlockWithFieldVerifying
                    {
                        Id = x.Id,
                        Name = x.Name,
                        OrderInParentCatalog = x.OrderInParentCatalog,
                        ParentCatalogId = x.ParentCatalogId,
                        Text = x.Text
                    }).ToList()
            };
            return View(catalogContent);
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            return View(new CatalogWithFieldVerifying
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog,
                ParentCatalogId = catalog.ParentCatalogId
            });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            return View(new CatalogWithFieldVerifying
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog,
                ParentCatalogId = catalog.ParentCatalogId
            });
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            return View(new CatalogWithFieldVerifying
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog,
                ParentCatalogId = catalog.ParentCatalogId
            });
        }

        [HttpPost]
        public IActionResult AddCatalog(CatalogWithFieldVerifying catalogWithFieldVerifying)
        {
            _directorySystemFacade.Add(new Catalog
            {
                Id = catalogWithFieldVerifying.Id,
                Name = catalogWithFieldVerifying.Name,
                OrderInParentCatalog = catalogWithFieldVerifying.OrderInParentCatalog,
                ParentCatalogId = catalogWithFieldVerifying.ParentCatalogId
            });
            return RedirectToAction("Index", new { id = catalogWithFieldVerifying.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteCatalog(int id)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            _directorySystemFacade.DeleteCatalog(catalog.Id);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateCatalog(CatalogWithFieldVerifying catalogWithFieldVerifying)
        {
            _directorySystemFacade.UpdateValue(new Catalog
            {
                Id = catalogWithFieldVerifying.Id,
                Name = catalogWithFieldVerifying.Name,
                OrderInParentCatalog = catalogWithFieldVerifying.OrderInParentCatalog,
                ParentCatalogId = catalogWithFieldVerifying.ParentCatalogId
            });
            return RedirectToAction("Index", new { id = catalogWithFieldVerifying.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult Move(int id, OrderAction action)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            _directorySystemFacade.UpdateOrder(catalog, action);
            return RedirectToAction("Index", new { id = catalog.ParentCatalogId });
        }
        [HttpGet]
        public IActionResult UpdateParentCatalog(int id)
        {
            Catalog catalog = _directorySystemFacade.GetCatalogById(id);
            return View(new CatalogWithFieldVerifying
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog,
                ParentCatalogId = catalog.ParentCatalogId
            });
        }

        [HttpPost]
        public IActionResult UpdateCatalogParentCatalog(CatalogWithFieldVerifying catalogWithFieldVerifying)
        {
            _directorySystemFacade.UpdateParentCatalog(new Catalog
            {
                Id = catalogWithFieldVerifying.Id,
                Name = catalogWithFieldVerifying.Name,
                OrderInParentCatalog = catalogWithFieldVerifying.OrderInParentCatalog,
                ParentCatalogId = catalogWithFieldVerifying.ParentCatalogId
            });
            return RedirectToAction("Index", "Catalog", new { id = catalogWithFieldVerifying.ParentCatalogId });
        }
    }
}