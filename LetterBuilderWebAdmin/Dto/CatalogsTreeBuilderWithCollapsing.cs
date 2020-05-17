using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LetterBuilderCore.Models;
using LetterBuilderCore.Services;

namespace LetterBuilderWebAdmin.Dto
{
    public class CatalogsTreeBuilderWithCollapsing : CatalogsTreeBuilder<CatalogNodeWithCollapsing>
    {
        private IDirectorySystemReadFacade _directoryFacade;
        public int SelectedCatalogId { get; set; }
        public CatalogsTreeBuilderWithCollapsing(IDirectorySystemReadFacade directorySystemFacade, int selectedCatalogId) : base(directorySystemFacade)
        {
            _directoryFacade = directorySystemFacade;
            SelectedCatalogId = selectedCatalogId;
        }

        protected override CatalogNodeWithCollapsing CreateNode(Catalog catalog, CatalogNodeWithCollapsing parentNode = default(CatalogNodeWithCollapsing))
        {
            CatalogNodeWithCollapsing catalogNode = new CatalogNodeWithCollapsing
            {
                Id = catalog.Id,
                Name = catalog.Name,
                Order = catalog.OrderInParentCatalog,
                ParentCatalog = parentNode
            };

            // Выставление флагов IsSelected и IsOpened в true у родительских каталогов, если
            // текущий рассматриваемый каталог является
            // каталогом, в котором в данный момент находится пользователь
            if (catalogNode.Id == SelectedCatalogId)
            {
                catalogNode.IsOpened = catalogNode.IsSelected = true;
                CatalogNodeWithCollapsing currParentNode = (CatalogNodeWithCollapsing) catalogNode.ParentCatalog;
                while (currParentNode != null)
                {
                    currParentNode.IsOpened = true;
                    currParentNode = (CatalogNodeWithCollapsing) currParentNode.ParentCatalog;
                }
            }
            return catalogNode;
        }
    }
}
