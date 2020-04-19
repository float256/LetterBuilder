using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderWebAdmin.Models;
using Microsoft.VisualStudio.Web.CodeGeneration;
using LetterBuilderWebAdmin.Services;

namespace LetterBuilderWebAdmin.Controllers
{
    public class TextBlockController : Controller
    {
        private IFileSystemRepository _fileSystemRepository;

        public TextBlockController(IFileSystemRepository fileSystemRepository)
        {
            _fileSystemRepository = fileSystemRepository;
        }

        [HttpGet]
        public IActionResult Add(int id) => View(new TextBlock { ParentCatalogId = id });

        [HttpGet]
        public IActionResult Delete(int id) => View(_fileSystemRepository.GetTextBlockById(id));

        [HttpGet]
        public IActionResult Update(int id) => View(_fileSystemRepository.GetTextBlockById(id));

        [HttpPost]
        public IActionResult AddTextBlock(TextBlock textBlock)
        {
            _fileSystemRepository.Add(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteTextBlock(int id)
        {
            TextBlock textBlock = _fileSystemRepository.GetTextBlockById(id);
            _fileSystemRepository.DeleteTextBlock(id);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateTextBlock(TextBlock textBlock)
        {
            _fileSystemRepository.UpdateValue(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        public IActionResult MoveUp(int id)
        {
            TextBlock textBlock = _fileSystemRepository.GetTextBlockById(id);
            _fileSystemRepository.UpdateOrder(textBlock, textBlock.OrderInParentCatalog - 1);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        public IActionResult MoveDown(int id)
        {
            TextBlock textBlock = _fileSystemRepository.GetTextBlockById(id);
            _fileSystemRepository.UpdateOrder(textBlock, textBlock.OrderInParentCatalog + 1);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }
    }
}