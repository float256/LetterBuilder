﻿using LetterBuilderCore.Models;
using LetterBuilderCore.Services;
using LetterBuilderCore.Services.DAO;
using LetterBuilderWebAdmin.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetterBuilderWebAdmin.Services
{
    public class ParsedCatalogsSaver : IParsedCatalogsSaver
    {
        private ICatalogDataAccess _catalogDataAccess;
        private ITextBlockDataAccess _textBlockDataAccess;

        public ParsedCatalogsSaver(ICatalogDataAccess catalogDataAccess, ITextBlockDataAccess textBlockDataAccess)
        {
            _catalogDataAccess = catalogDataAccess;
            _textBlockDataAccess = textBlockDataAccess;
        }

        /// <summary>
        /// Данный метод сохраняет переданное дерево каталогов в базу данных
        /// </summary>
        /// <param name="catalogNode">Корневой каталог в переданном дереве каталогов. 
        /// У данного каталога должен быть указан Id. Id подкаталогов и текстовых файлов вычисляются при добавлении</param>
        public void AddCatalogTree(CatalogParserNodeDto catalogNode)
        {
            List<CatalogParserNodeDto> catalogsOnCurrentDepthLevel = catalogNode.ChildrenNodes;
            foreach (CatalogParserNodeDto item in catalogsOnCurrentDepthLevel)
            {
                item.ParentCatalogId = catalogNode.Id;
            }
            foreach (TextBlock item in catalogNode.CatalogAttachments)
            {
                item.ParentCatalogId = catalogNode.Id;
                _textBlockDataAccess.Add(item);
            }
            while (catalogsOnCurrentDepthLevel.Count > 0)
            {
                List<CatalogParserNodeDto> catalogsOnNextDepthLevel = new List<CatalogParserNodeDto>();
                foreach (var node in catalogsOnCurrentDepthLevel)
                {
                    Catalog catalog = new Catalog
                    {
                        Name = node.Name,
                        OrderInParentCatalog = node.Order,
                        ParentCatalogId = node.ParentCatalogId
                    };
                    _catalogDataAccess.Add(catalog);
                    node.Id = catalog.Id;
                    foreach (CatalogParserNodeDto item in node.ChildrenNodes)
                    {
                        item.ParentCatalogId = node.Id;
                        catalogsOnNextDepthLevel.Add(item);
                    }
                    foreach (TextBlock item in node.CatalogAttachments)
                    {
                        item.ParentCatalogId = node.Id;
                        _textBlockDataAccess.Add(item);
                    }
                }
                catalogsOnCurrentDepthLevel = catalogsOnNextDepthLevel;
            }
        }
    }
}
