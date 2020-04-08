using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LetterBuilderWebAdmin.Models;

namespace LetterBuilderWebAdmin.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index(int id)
        {
            CatalogRepository catalogRepository = new CatalogRepository("Data Source=DESKTOP-2U3C1KN\\SQLEXPRESS;Initial Catalog=DirectoryStructure;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            
            return View(catalogRepository.GetCatalogContent(id));
        }
    }
}