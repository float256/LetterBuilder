using LetterBuilderCore.Models;
using LetterBuilderCore.Services;
using LetterBuilderCore.Services.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderCore.Models
{
    public class CatalogsTreeBuilder<NodeType> where NodeType: ICatalogNode, new()
    {
        private IDirectorySystemReadFacade _directoryFacade;

        public CatalogsTreeBuilder(IDirectorySystemReadFacade directorySystemFacade)
        {
            _directoryFacade = directorySystemFacade;
        }

        /// <summary>
        /// Данный метод строит дерево каталогов
        /// </summary>
        /// <param name="id">Id каталога, с которого нужно начинать строить дерево</param>
        /// <param name="isShowAllCatalogs">Если true, то в дереве будут все каталоги, 
        /// иначе построение дерева начнется с каталога, id которого равен переданному id</param>
        /// <returns>Объект типа ICatalogNode, который является начальным каталогом</returns>
        public NodeType BuildTree(int id, bool isShowAllCatalogs = false)
        {
            int initialId = isShowAllCatalogs ? 0 : id;
            Catalog initalCatalog = _directoryFacade.GetCatalogById(initialId);
            NodeType result = CreateNode(initalCatalog, id);

            // Получение подкаталогов и файлов, находящихся в изначальном каталоге
            foreach (Catalog subcatalog in _directoryFacade.GetSubcatalogs(initialId))
            {
                result.ChildrenNodes.Add(CreateNode(subcatalog, id));
            }

            // Построение дерева
            List<ICatalogNode> catalogsOnCurrDepthLevel = Enumerable.ToList<ICatalogNode>(result.ChildrenNodes);
            while (catalogsOnCurrDepthLevel.Count > 0)
            {
                List<ICatalogNode> catalogsOnNextDepthLevel = new List<ICatalogNode>();
                foreach (NodeType currParentCatalog in catalogsOnCurrDepthLevel)
                {
                    List<Catalog> allSubcatalogs = _directoryFacade.GetSubcatalogs(currParentCatalog.Id);
                    foreach (Catalog currSubcatalog in allSubcatalogs)
                    {
                        NodeType node = CreateNode(currSubcatalog, id);
                        currParentCatalog.ChildrenNodes.Add(node);
                        catalogsOnNextDepthLevel.Add(node);
                    }
                }
                catalogsOnCurrDepthLevel = catalogsOnNextDepthLevel;
            }
            return result;
        }

        protected virtual NodeType CreateNode(Catalog catalog, int id)
        {
            return new NodeType
            {
                Id = catalog.Id,
                Name = catalog.Name,
                Order = catalog.OrderInParentCatalog,
                CatalogAttachments = _directoryFacade.GetCatalogAttachments(catalog.Id)
            };
        }
    }
}