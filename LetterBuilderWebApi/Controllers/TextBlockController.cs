using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderCore.Models;
using LetterBuilderCore.Services;
using LetterBuilderWebApi.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetterBuilderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextBlockController : ControllerBase
    {
        private IDirectorySystemReadFacade _directoryFacade;
        public TextBlockController(IDirectorySystemReadFacade directoryFacade)
        {
            _directoryFacade = directoryFacade;
        }

        [HttpGet("{id}")]
        public ActionResult<TextBlockDto> GetTextBlockInfo(int id)
        {
            TextBlock textBlock = _directoryFacade.GetTextBlockById(id);
            return Ok(new TextBlockDto
            { 
                Id = textBlock.Id,
                Text = textBlock.Text,
                Name = textBlock.Name,
                OrderInParentCatalog = textBlock.OrderInParentCatalog
            });
        }
    }
}
