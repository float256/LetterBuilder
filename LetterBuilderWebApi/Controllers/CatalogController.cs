using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderWebAdmin.Models;
using LetterBuilderWebAdmin.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LetterBuilderWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private IDirectorySystemFacade _directoryFacade;

        public CatalogController(IDirectorySystemFacade directoryFacade)
        {
            _directoryFacade = directoryFacade;
        }

        [Route("{id}")]
        public ActionResult<Catalog> GetCatalogInfo(int id)
        {
            return _directoryFacade.GetCatalogById(id);
        }

        [Route("subcatalogs/{id}")]
        public ActionResult<List<Catalog>> GetSubcatalogs(int parentCatalogId)
        {
            return _directoryFacade.GetSubcatalogs(parentCatalogId);
        }

        [Route("catalogattachments/{id}")]
        public ActionResult<List<TextBlock>> GetCatalogAttachments(int parentCatalogId)
        {
            return _directoryFacade.GetCatalogAttachments(parentCatalogId);
        }

        [Route("firsttwonestinglevels/")]
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
            return topLevelCatalogNodes;
        }

        [Route("gettree/{id}")]
        public ActionResult<CatalogNode> GetTree(int id)
        {
            CatalogsTreeBuilder tree = new CatalogsTreeBuilder(_directoryFacade);
            return tree.BuildTree(id);
        }
    }
}