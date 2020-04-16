using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using LetterBuilderWebAdmin.Models;


namespace LetterBuilderWebAdmin.Controllers
{
    public class TextBlockController : Controller
    {
        private string _connectionString;
        private TextBlockRepository _textBlockRepository;

        public TextBlockController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
            _textBlockRepository = new TextBlockRepository(_connectionString);
        }

        [HttpGet]
        public IActionResult Add(int id) => View(new TextBlock { ParentCatalogId = id });

        [HttpGet]
        public IActionResult Delete(int id) => View(_textBlockRepository.GetById(id));

        [HttpGet]
        public IActionResult Update(int id) => View(_textBlockRepository.GetById(id));

        [HttpPost]
        public IActionResult AddTextBlock(TextBlock textBlock)
        {
            _textBlockRepository.Add(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult DeleteTextBlock(int id)
        {
            TextBlock textBlock = _textBlockRepository.GetById(id);
            _textBlockRepository.Delete(id);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }

        [HttpPost]
        public IActionResult UpdateTextBlock(TextBlock textBlock)
        {
            _textBlockRepository.Update(textBlock);
            return RedirectToAction("Index", "Catalog", new { id = textBlock.ParentCatalogId });
        }
    }
}