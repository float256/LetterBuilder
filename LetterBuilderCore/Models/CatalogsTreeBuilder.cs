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
        /// <param name="isAddAllCatalogs">Если true, то в дереве будут все каталоги, 
        /// иначе построение дерева начнется с каталога, id которого равен переданному id</param>
        /// <param name="isAddTextBlocks">Если true, то в дерево будут добавлены текстовые файлы</param>
        /// <returns>Объект типа NodeType, который является начальным каталогом</returns>
        public NodeType BuildTree(int id, bool isAddAllCatalogs = false, bool isAddTextBlocks = false)
        {
            int initialId = isAddAllCatalogs ? 0 : id;
            Catalog initialCatalog;
            if (initialId == 0)
            {
                initialCatalog = new Catalog { Id = 0 };
            }
            else
            {
                initialCatalog = _directoryFacade.GetCatalogById(initialId);
                if (initialCatalog == null)
                {
                    return new NodeType();
                }
            }
            NodeType result = CreateNode(initialCatalog, id);

            // Получение подкаталогов и файлов, находящихся в изначальном каталоге
            foreach (Catalog subcatalog in _directoryFacade.GetSubcatalogs(initialId))
            {
                result.ChildrenNodes.Add(CreateNode(subcatalog, id, result));
            }
            if (isAddTextBlocks)
            {
                foreach (TextBlock textBlock in _directoryFacade.GetCatalogAttachments(initialId))
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
                        NodeType node = CreateNode(currSubcatalog, id, currParentCatalog);
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

        protected virtual NodeType CreateNode(Catalog catalog, int id, NodeType parentNode = default(NodeType))
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