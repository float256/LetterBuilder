using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderWebAdmin.Models;
using LetterBuilderWebAdmin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetterBuilderWebApi.Services.DAO
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextBlockController : ControllerBase
    {
        private IDirectorySystemFacade _directoryFacade;
        public TextBlockController(IDirectorySystemFacade directoryFacade)
        {
            _directoryFacade = directoryFacade;
        }

        [Route("{id}")]
        public ActionResult<TextBlock> GetTextBlockInfo(int id)
        {
            return _directoryFacade.GetTextBlockById(id);
        }
    }
}