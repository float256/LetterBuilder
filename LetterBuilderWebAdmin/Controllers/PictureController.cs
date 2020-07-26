using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderCore.Models;
using LetterBuilderCore.Services;
using LetterBuilderWebAdmin.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace LetterBuilderWebAdmin.Controllers
{
    public class PictureController : Controller
    {
        private IDirectorySystemFacade _directorySystemFacade;
        public PictureController(IDirectorySystemFacade directorySystemFacade)
        {
            _directorySystemFacade = directorySystemFacade;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(IFormFile upload)
        {
            PictureResizer pictureResizer = new PictureResizer();
            Bitmap bitmap = new Bitmap(upload.OpenReadStream());
            Picture picture = new Picture { BinaryData = pictureResizer.GetPictureBinaryData(bitmap) };
            _directorySystemFacade.Add(picture);
            return Json(new Dictionary<string, string>
            {
                { "uploaded", "true" },
                { "url", $"/Admin/Picture/Get/{picture.Id.ToString()}" },
                { "width", "100%" },
                { "height", "auto" },
            });
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            Picture picture = _directorySystemFacade.GetPictureById(id);
            return base.File(picture.BinaryData, "image/jpg");
        }
    }
}
