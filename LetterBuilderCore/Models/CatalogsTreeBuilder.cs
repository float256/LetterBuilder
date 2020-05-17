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
        /// <param name="initialCatalogId">Id каталога, с которого нужно начинать строить дерево</param>
        /// <param name="isAddTextBlocks">Если true, то в дерево будут добавлены текстовые файлы</param>
        /// <returns>Объект типа NodeType, который является начальным каталогом</returns>
        public NodeType BuildTree(int initialCatalogId = Constants.RootCatalogId, bool isAddTextBlocks = false)
        {
            Catalog initialCatalog;
            if (initialCatalogId == 0)
            {
                initialCatalog = new Catalog { Id = 0 };
            }
            else
            {
                initialCatalog = _directoryFacade.GetCatalogById(initialCatalogId);
                if (initialCatalog == null)
                {
                    return new NodeType();
                }
            }
            NodeType result = CreateNode(initialCatalog);

            // Получение подкаталогов и файлов, находящихся в изначальном каталоге
            foreach (Catalog subcatalog in _directoryFacade.GetSubcatalogs(initialCatalogId))
            {
                result.ChildrenNodes.Add(CreateNode(subcatalog, result));
            }
            if (isAddTextBlocks)
            {
                foreach (TextBlock textBlock in _directoryFacade.GetCatalogAttachments(initialCatalogId))
                {
                    result.CatalogAttachments.Add(textBlock);
                }
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
                        NodeType node = CreateNode(currSubcatalog, currParentCatalog);
                        currParentCatalog.ChildrenNodes.Add(node);
                        catalogsOnNextDepthLevel.Add(node);
                    }

                    if (isAddTextBlocks)
                    {
                        foreach (TextBlock textBlock in _directoryFacade.GetCatalogAttachments(currParentCatalog.Id))
                        {
                            currParentCatalog.CatalogAttachments.Add(textBlock);
                        }
                    }
                }
                catalogsOnCurrDepthLevel = catalogsOnNextDepthLevel;
            }
            return result;
        }

        protected virtual NodeType CreateNode(Catalog catalog, NodeType parentNode = default(NodeType))
        {
            return new NodeType
            {
                Id = catalog.Id,
                Name = catalog.Name,
                Order = catalog.OrderInParentCatalog,
                ParentCatalog = null
            };
        }
    }
}