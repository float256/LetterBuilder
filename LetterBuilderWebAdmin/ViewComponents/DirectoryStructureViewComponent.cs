using LetterBuilderWebAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;

namespace LetterBuilderWebAdmin.ViewComponents
{
    public class DirectoryStructureViewComponent : ViewComponent
    {
        private string _connectionString ;
        private TextBlockRepository _textBlockRepository;
        private CatalogRepository _catalogRepository;

        public DirectoryStructureViewComponent(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("default");
            _textBlockRepository = new TextBlockRepository(_connectionString);
            _catalogRepository = new CatalogRepository(_connectionString);
        }
        public IViewComponentResult Invoke(int id)
        {
            CatalogContent result = new CatalogContent
            {
                Catalogs = _catalogRepository.GetSubcatalogsByParentCatalogId(id),
                TextBlocks = _textBlockRepository.GetTextBlocksByParentCatalogId(id)
            };
            return View(result);
        }
    }
}
