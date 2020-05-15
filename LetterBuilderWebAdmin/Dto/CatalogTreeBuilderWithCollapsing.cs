using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderCore.Models;
using LetterBuilderCore.Services;

namespace LetterBuilderWebAdmin.Dto
{
    public class CatalogTreeBuilderWithCollapsing : CatalogsTreeBuilder<CatalogNodeWithCollapsing>
    {
        private IDirectorySystemReadFacade _directoryFacade;
        public CatalogTreeBuilderWithCollapsing(IDirectorySystemReadFacade directorySystemFacade) : base(directorySystemFacade)
        {
            _directoryFacade = directorySystemFacade;
        }

        protected override CatalogNodeWithCollapsing CreateNode(Catalog catalog, int id)
        {
            CatalogNodeWithCollapsing catalogNode = new CatalogNodeWithCollapsing
            {
                Id = catalog.Id,
                Name = catalog.Name,
                Order = catalog.OrderInParentCatalog,
                CatalogAttachments = _directoryFacade.GetCatalogAttachments(catalog.Id)
            };

            // Выставление флагов IsSelected и IsOpened в true у родительских каталогов, если
            // текущий рассматриваемый каталог является
            // каталогом, в котором в данный момент находится пользователь
            if (catalogNode.Id == id)
            {
                catalogNode.IsOpened = catalogNode.IsSelected = true;
                CatalogNodeWithCollapsing parentNode = (CatalogNodeWithCollapsing) catalogNode.ParentCatalog;
                while (parentNode != null)
                {
                    parentNode.IsOpened = true;
                    parentNode = (CatalogNodeWithCollapsing) catalogNode.ParentCatalog;
                }
            }
            return catalogNode;
        }
    }
}
