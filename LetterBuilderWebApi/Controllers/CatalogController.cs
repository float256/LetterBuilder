using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderCore.Models;
using LetterBuilderCore.Services;
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
        public ActionResult<Catalog> GetCatalogInfo(int id)
        {
            return Ok(_directoryFacade.GetCatalogById(id));
        }

        [HttpGet("SubCatalogs/{id}")]
        public ActionResult<List<Catalog>> GetSubcatalogs(int parentCatalogId)
        {
            return Ok(_directoryFacade.GetSubcatalogs(parentCatalogId));
        }

        [HttpGet("CatalogAttachments/{id}")]
        public ActionResult<List<TextBlock>> GetCatalogAttachments(int parentCatalogId)
        {
            return Ok(_directoryFacade.GetCatalogAttachments(parentCatalogId));
        }

        [HttpGet("FirstTwoNestingLevels/")]
        public ActionResult<List<CatalogNode>> GetFirstTwoNestingLevels()
        {
            List<CatalogNode> topLevelCatalogNodes = new List<CatalogNode>();
            foreach (Catalog currTopCatalog in _directoryFacade.GetSubcatalogs(0))
            {
                CatalogNode currTopCatalogNode = new CatalogNode
                {
                    Id = currTopCatalog.Id,
                    Name = currTopCatalog.Name,
                    Order = currTopCatalog.OrderInParentCatalog
                };
                topLevelCatalogNodes.Add(currTopCatalogNode);
                foreach (Catalog currSubcatalog in _directoryFacade.GetSubcatalogs(currTopCatalog.Id))
                {
                    CatalogNode currSubcatalogNode = new CatalogNode
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
        public ActionResult<CatalogNode> GetTree(int id)
        {
            CatalogsTreeBuilder<CatalogNode> treeBuilder = new CatalogsTreeBuilder<CatalogNode>(_directoryFacade);
            var tree = treeBuilder.BuildTree(id, isAddTextBlocks: true);
            return tree;
        }
    }
}