using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderCore.Services;
using LetterBuilderCore.Models;
using LetterBuilderWebAdmin.Dto;
using LetterBuilderWebAdmin.Services;

namespace LetterBuilderWebAdmin.Controllers
{
    public class CatalogController : Controller
    {
        private IDirectorySystemFacade _directorySystemFacade;
        private IParsedCatalogsSaver _parsedCatalogsSaver;

        public CatalogController(IDirectorySystemFacade directorySystemFacade, IParsedCatalogsSaver parsedCatalogsSaver)
        {
            _directorySystemFacade = directorySystemFacade;
            _parsedCatalogsSaver = parsedCatalogsSaver;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            CatalogContent catalogContent = new CatalogContent
            {
                Catalogs = _directorySystemFacade.GetSubcatalogs(id).Select(
                    x => new CatalogDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        OrderInParentCatalog = x.OrderInParentCatalog,
                        ParentCatalogId = x.ParentCatalogId
                    }).ToList(),
                TextBlocks = _directorySystemFacade.GetCatalogAttachments(id).Select(
                    x => new TextBlockDto
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
            return View(new CatalogDto
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
            return View(new CatalogDto
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
            return View(new CatalogDto
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog,
                ParentCatalogId = catalog.ParentCatalogId
            });
        }

        [HttpPost]
        public IActionResult AddCatalog(CatalogDto catalogWithFieldVerifying)
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
        public IActionResult UpdateCatalog(CatalogDto catalogWithFieldVerifying)
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
            return View(new CatalogDto
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog,
                ParentCatalogId = catalog.ParentCatalogId
            });
        }

        [HttpPost]
        public IActionResult UpdateCatalogParentCatalog(CatalogDto catalogWithFieldVerifying)
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

        [HttpGet]
        public IActionResult CatalogParser(int id)
        {
            return View(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddParseCatalogs([FromBody] CatalogParserNodeDto catalogNode)
        {
            _parsedCatalogsSaver.AddCatalogTree(catalogNode);
            return RedirectToAction("Index", "Catalog", new { id = catalogNode.Id });
        }
    }
}