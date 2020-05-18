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
    public class CatalogController : ControllerBase
    {
        private IDirectorySystemReadFacade _directoryFacade;

        public CatalogController(IDirectorySystemReadFacade directoryFacade)
        {
            _directoryFacade = directoryFacade;
        }

        [HttpGet("{id}")]
        public ActionResult<CatalogDto> GetCatalogInfo(int id)
        {
            Catalog catalog = _directoryFacade.GetCatalogById(id);
            return Ok(new CatalogDto
            {
                Id = catalog.Id,
                Name = catalog.Name,
                OrderInParentCatalog = catalog.OrderInParentCatalog
            });
        }

        [HttpGet("SubCatalogs/{id}")]
        public ActionResult<List<CatalogDto>> GetSubcatalogs(int parentCatalogId)
        {
            List<Catalog> catalogs = _directoryFacade.GetSubcatalogs(parentCatalogId);
            return Ok(catalogs.Select(x => new CatalogDto 
            {
                Id = x.Id,
                Name = x.Name,
                OrderInParentCatalog = x.OrderInParentCatalog
            }).ToList());
        }

        [HttpGet("CatalogAttachments/{id}")]
        public ActionResult<List<TextBlockDto>> GetCatalogAttachments(int parentCatalogId)
        {
            List<TextBlock> textBlocks = _directoryFacade.GetCatalogAttachments(parentCatalogId);
            return Ok(textBlocks.Select(x => new TextBlockDto
            {
                Id = x.Id,
                Name = x.Name,
                OrderInParentCatalog = x.OrderInParentCatalog,
                Text = x.Text
            }).ToList());
        }

        [HttpGet("FirstTwoNestingLevels/")]
        public ActionResult<List<CatalogNodeDto>> GetFirstTwoNestingLevels()
        {
            List<CatalogNodeDto> topLevelCatalogNodes = new List<CatalogNodeDto>();
            foreach (Catalog currTopCatalog in _directoryFacade.GetSubcatalogs(0))
            {
                CatalogNodeDto currTopCatalogNode = new CatalogNodeDto
                {
                    Id = currTopCatalog.Id,
                    Name = currTopCatalog.Name,
                    Order = currTopCatalog.OrderInParentCatalog
                };
                topLevelCatalogNodes.Add(currTopCatalogNode);
                foreach (Catalog currSubcatalog in _directoryFacade.GetSubcatalogs(currTopCatalog.Id))
                {
                    CatalogNodeDto currSubcatalogNode = new CatalogNodeDto
                    {
                        Id = currSubcatalog.Id,
                        Name = currSubcatalog.Name,
                        Order = currSubcatalog.OrderInParentCatalog
                    };
                    currTopCatalogNode.ChildrenNodes.Add(currSubcatalogNode);
                }
            }
            return Ok(topLevelCatalogNodes);
        }

        [HttpGet("GetTree/{id}")]
        public ActionResult<CatalogNodeDto> GetTree(int id)
        {
            CatalogsTreeBuilder<CatalogNodeDto> treeBuilder = new CatalogsTreeBuilder<CatalogNodeDto>(_directoryFacade);
            var tree = treeBuilder.BuildTree(id, isAddTextBlocks: true);
            return tree;
        }
    }
}