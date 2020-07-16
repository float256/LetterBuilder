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
    public class TextBlockController : Controller
    {
        private IDirectorySystemFacade _directorySystemFacade;

        public TextBlockController(IDirectorySystemFacade directorySystemFacade)
        {
            _directorySystemFacade = directorySystemFacade;
        }

        [HttpGet]
        public IActionResult Add(int id)
        {
            return View(new TextBlockDto { ParentCatalogId = id });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            TextBlock textBlock = _directorySystemFacade.GetTextBlockById(id);
            return View(new TextBlockDto
            {
                Id = textBlock.Id,
                Name = textBlock.Name,
                OrderInParentCatalog = textBlock.OrderInParentCatalog,
                ParentCatalogId = textBlock.ParentCatalogId,
                Text = textBlock.Text
            });
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            TextBlock textBlock = _directorySystemFacade.GetTextBlockById(id);
            return View(new TextBlockDto
            { 
                Id = textBlock.Id,
                Name = textBlock.Name,
                OrderInParentCatalog = textBlock.OrderInParentCatalog,
                ParentCatalogId = textBlock.ParentCatalogId,
                Text = textBlock.Text
            });
        }

        [HttpPost]
        public IActionResult AddTextBlock(TextBlockDto textBlockWithFieldVerifying)
        {
            _directorySystemFacade.Add(new TextBlock
            {
                Id = textBlockWithFieldVerifying.Id,
                Name = textBlockWithFieldVerifying.Name,
                OrderInParentCatalog = textBlockWithFieldVerifying.OrderInParentCatalog,
                ParentCatalogId = textBlockWithFieldVerifying.ParentCatalogId,
                Text = textBlockWithFieldVerifying.Text
            });
            return RedirectToAction("Index", "Catalog", new { id = textBlockWithFieldVerifying.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteTextBlock(int id)
        {
            LetterBuilderCore.Models.TextBlock textBlock = _directorySystemFacade.GetTextBlockById(id);
            _directorySystemFacade.DeleteTextBlock(id);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateTextBlock(TextBlockDto textBlockWithFieldVerifying)
        {
            _directorySystemFacade.UpdateValue(new TextBlock
            {
                Id = textBlockWithFieldVerifying.Id,
                Name = textBlockWithFieldVerifying.Name,
                OrderInParentCatalog = textBlockWithFieldVerifying.OrderInParentCatalog,
                ParentCatalogId = textBlockWithFieldVerifying.ParentCatalogId,
                Text = textBlockWithFieldVerifying.Text
            });
            return RedirectToAction("Index", "Catalog", new { id = textBlockWithFieldVerifying.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult Move(int id, OrderAction action)
        {
            TextBlock textBlock = _directorySystemFacade.GetTextBlockById(id);
            _directorySystemFacade.UpdateOrder(textBlock, action);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpGet]
        public IActionResult UpdateParentCatalog(int id)
        {
            TextBlock textBlock = _directorySystemFacade.GetTextBlockById(id);
            return View(new TextBlockDto
            {
                Id = textBlock.Id,
                Name = textBlock.Name,
                OrderInParentCatalog = textBlock.OrderInParentCatalog,
                ParentCatalogId = textBlock.ParentCatalogId,
                Text = textBlock.Text
            });
        }

        [HttpPost]
        public IActionResult UpdateTextBlockParentCatalog(TextBlockDto textBlockWithFieldVerifying)
        {
            _directorySystemFacade.UpdateParentCatalog(new TextBlock
            {
                Id = textBlockWithFieldVerifying.Id,
                Name = textBlockWithFieldVerifying.Name,
                OrderInParentCatalog = textBlockWithFieldVerifying.OrderInParentCatalog,
                ParentCatalogId = textBlockWithFieldVerifying.ParentCatalogId,
                Text = textBlockWithFieldVerifying.Text
            });
            return RedirectToAction("Index", "Catalog", new { id = textBlockWithFieldVerifying.ParentCatalogId });
        }
    }
}