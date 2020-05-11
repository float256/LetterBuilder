﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderWebAdmin.Models;
using Microsoft.VisualStudio.Web.CodeGeneration;
using LetterBuilderWebAdmin.Services;
using Microsoft.AspNetCore.Razor.Language;

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
            return View(new TextBlock { ParentCatalogId = id });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            return View(_directorySystemFacade.GetTextBlockById(id));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            return View(_directorySystemFacade.GetTextBlockById(id));
        }

        [HttpPost]
        public IActionResult AddTextBlock(TextBlock textBlock)
        {
            _directorySystemFacade.Add(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteTextBlock(int id)
        {
            TextBlock textBlock = _directorySystemFacade.GetTextBlockById(id);
            _directorySystemFacade.DeleteTextBlock(id);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateTextBlock(TextBlock textBlock)
        {
            _directorySystemFacade.UpdateValue(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
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
            return View(_directorySystemFacade.GetTextBlockById(id));
        }

        [HttpPost]
        public IActionResult UpdateTextBlockParentCatalog(TextBlock textBlock)
        {
            int maxOrder = 0;
            foreach (TextBlock item in _directorySystemFacade.GetCatalogAttachments(textBlock.ParentCatalogId))
            {
                if (maxOrder < item.OrderInParentCatalog)
                {
                    maxOrder = item.OrderInParentCatalog;
                }
            }
            foreach (Catalog item in _directorySystemFacade.GetSubcatalogs(textBlock.ParentCatalogId))
            {
                if (maxOrder < item.OrderInParentCatalog)
                {
                    maxOrder = item.OrderInParentCatalog;
                }
            }
            textBlock.OrderInParentCatalog = maxOrder + 1;
            _directorySystemFacade.UpdateParentCatalog(textBlock);
            _directorySystemFacade.UpdateOrder(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }
    }
}