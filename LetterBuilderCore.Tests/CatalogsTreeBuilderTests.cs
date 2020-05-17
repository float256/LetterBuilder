using System;
using Xunit;
using Moq;
using LetterBuilderCore.Services;
using LetterBuilderCore.Models;
using System.Collections.Generic;

namespace LetterBuilderCore.Tests
{
    public class CatalogsTreeBuilderTests
    {
        [Fact]
        public void CatalogsTreeBuilder_BuildTree_DirectoryFacadeWithoutElements_EmptyTree()
        {
            // Arrange

            Mock<IDirectorySystemFacade> mockDirectorySystemFacade = new Mock<IDirectorySystemFacade>(MockBehavior.Strict);
            mockDirectorySystemFacade.Setup(m => m.GetSubcatalogs(It.IsAny<int>())).Returns<int>(x => new List<Catalog>());
            mockDirectorySystemFacade.Setup(m => m.GetCatalogById(It.IsAny<int>())).Returns<int>(x => new Catalog());

            // Act

            CatalogsTreeBuilder<CatalogNode> catalogsTreeBuilder = new CatalogsTreeBuilder<CatalogNode>(mockDirectorySystemFacade.Object);
            CatalogNode tree = catalogsTreeBuilder.BuildTree(0);

            // Assert

            Assert.Empty(tree.ChildrenNodes);
            Assert.Empty(tree.CatalogAttachments);
            Mock.VerifyAll();
        }

        [Fact]
        public void CatalogsTreeBuilder_BuildTree_DirectoryFacadeWithOnlyCatalogs_CorrectlyBuildTree()
        {
            // Arrange

            List<Catalog> catalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", ParentCatalogId = 0, OrderInParentCatalog = 1 },
                new Catalog{ Id = 2, Name = "Catalog2", ParentCatalogId = 0, OrderInParentCatalog = 2 },
                new Catalog{ Id = 3, Name = "SubCatalog1", ParentCatalogId = 1, OrderInParentCatalog = 1 },
                new Catalog{ Id = 4, Name = "SubCatalog2", ParentCatalogId = 1, OrderInParentCatalog = 2 },
                new Catalog{ Id = 5, Name = "SubSubCatalog1", ParentCatalogId = 3, OrderInParentCatalog = 1 }
            };
            Mock<IDirectorySystemFacade> mock = new Mock<IDirectorySystemFacade>(MockBehavior.Strict);
            mock.Setup(m => m.GetCatalogAttachments(It.IsAny<int>())).Returns<int>(x => new List<TextBlock>());
            foreach (Catalog catalog in catalogs)
            {
                List<Catalog> currSubcatalogs = new List<Catalog>();
                foreach (Catalog subcatalog in catalogs.FindAll(x => x.ParentCatalogId == catalog.Id))
                {
                    currSubcatalogs.Add(subcatalog);
                }
                mock.Setup(m => m.GetSubcatalogs(It.Is<int>(id => id == catalog.Id))).Returns<int>(x => currSubcatalogs);
                mock.Setup(m => m.GetCatalogById(It.Is<int>(id => id == catalog.Id))).Returns<int>(x => catalog);
            }
            mock.Setup(m => m.GetSubcatalogs(It.Is<int>(id => id > 5))).Returns<int>(x => new List<Catalog>());
            mock.Setup(m => m.GetSubcatalogs(It.Is<int>(id => id == 0)))
                .Returns<int>(x => catalogs.FindAll(catalog => catalog.ParentCatalogId == 0));

            // Act

            CatalogNode expectedTree = new CatalogNode
            {
                ChildrenNodes = new List<ICatalogNode>
                {
                    new CatalogNode
                    {
                        Id = 1, Name = "Catalog1", Order = 1,
                        ChildrenNodes = new List<ICatalogNode>
                        {
                            new CatalogNode
                            {
                                Id = 3, Name = "SubCatalog1", Order = 1,
                                ChildrenNodes = new List<ICatalogNode>
                                {
                                    new CatalogNode
                                    {
                                        Id = 5, Name = "SubSubCatalog1", Order = 1,
                                        ChildrenNodes = new List<ICatalogNode>()
                                    }
                                },
                            },
                            new CatalogNode
                            {
                                Id = 4, Name = "SubCatalog2", Order = 2,
                                ChildrenNodes = new List<ICatalogNode>()
                            }
                        },
                    },
                    new CatalogNode
                    {
                        Id = 2, Name = "Catalog2", Order = 2,
                        ChildrenNodes = new List<ICatalogNode>()
                    }
                }
            };
            CatalogsTreeBuilder<CatalogNode> catalogsTreeBuilder = new CatalogsTreeBuilder<CatalogNode>(mock.Object);
            CatalogNode tree = catalogsTreeBuilder.BuildTree(0, true, false);

            // Assert

            List<ICatalogNode> expectedNodesOnCurrDepthLevel = expectedTree.ChildrenNodes;
            List<ICatalogNode> actualNodesOnCurrDepthLevel = tree.ChildrenNodes;
            while (expectedNodesOnCurrDepthLevel.Count > 0 && actualNodesOnCurrDepthLevel.Count > 0)
            {
                List<ICatalogNode> expectedNodesOnNextDepthLevel = new List<ICatalogNode>();
                List<ICatalogNode> actualNodesOnNextDepthLevel = new List<ICatalogNode>();
                Assert.Equal(expectedNodesOnCurrDepthLevel.Count, actualNodesOnCurrDepthLevel.Count);
                for (int i = 0; i < expectedNodesOnCurrDepthLevel.Count; i++)
                {
                    ICatalogNode expectedCatalogNode = expectedNodesOnCurrDepthLevel[i];
                    ICatalogNode actualCatalogNode = actualNodesOnCurrDepthLevel[i];
                    expectedNodesOnNextDepthLevel.AddRange(expectedCatalogNode.ChildrenNodes);
                    actualNodesOnNextDepthLevel.AddRange(actualCatalogNode.ChildrenNodes);

                    Assert.Equal(expectedCatalogNode.Id, actualCatalogNode.Id);
                    Assert.Equal(expectedCatalogNode.Name, actualCatalogNode.Name);
                    Assert.Equal(expectedCatalogNode.Order, actualCatalogNode.Order);
                }
                expectedNodesOnCurrDepthLevel = expectedNodesOnNextDepthLevel;
                actualNodesOnCurrDepthLevel = actualNodesOnNextDepthLevel;
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void CatalogsTreeBuilder_BuildTree_DirectoryFacadeWithCatalogsAndTextBlocks_CorrectlyBuildedTree()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", ParentCatalogId = 0, OrderInParentCatalog = 1 },
                new Catalog{ Id = 2, Name = "Catalog2", ParentCatalogId = 0, OrderInParentCatalog = 2 },
                new Catalog{ Id = 3, Name = "SubCatalog1", ParentCatalogId = 1, OrderInParentCatalog = 1 },
                new Catalog{ Id = 4, Name = "SubCatalog2", ParentCatalogId = 1, OrderInParentCatalog = 2 },
                new Catalog{ Id = 5, Name = "SubSubCatalog1", ParentCatalogId = 3, OrderInParentCatalog = 1 }
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 1, Name = "TextBlock1", Text = "Text1", ParentCatalogId = 0, OrderInParentCatalog = 3},
                new TextBlock{ Id = 2, Name = "TextBlock2", Text = "Text2", ParentCatalogId = 1, OrderInParentCatalog = 3},
                new TextBlock{ Id = 3, Name = "TextBlock3", Text = "Text3", ParentCatalogId = 3, OrderInParentCatalog = 2},
                new TextBlock{ Id = 4, Name = "TextBlock4", Text = "Text4", ParentCatalogId = 4, OrderInParentCatalog = 1}
            };
            Mock<IDirectorySystemFacade> mock = new Mock<IDirectorySystemFacade>(MockBehavior.Strict);
            foreach (Catalog catalog in allCatalogs)
            {
                List<Catalog> currSubcatalogs = new List<Catalog>();
                List<TextBlock> currTextBlocks = new List<TextBlock>();
                foreach (Catalog subcatalog in allCatalogs.FindAll(x => x.ParentCatalogId == catalog.Id))
                {
                    currSubcatalogs.Add(subcatalog);
                }
                foreach (TextBlock textBlock in allTextBlocks.FindAll(x => x.ParentCatalogId == catalog.Id))
                {
                    currTextBlocks.Add(textBlock);
                }
                mock.Setup(m => m.GetSubcatalogs(It.Is<int>(id => id == catalog.Id))).Returns<int>(x => currSubcatalogs);
                mock.Setup(m => m.GetCatalogAttachments(It.Is<int>(id => id == catalog.Id))).Returns<int>(x => currTextBlocks);
                mock.Setup(m => m.GetCatalogById(It.Is<int>(id => id == catalog.Id))).Returns<int>(x => catalog);
            }
            mock.Setup(m => m.GetSubcatalogs(It.Is<int>(id => id == 0)))
                .Returns<int>(x => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == 0));
            mock.Setup(m => m.GetSubcatalogs(It.Is<int>(id => id > 5))).Returns<int>(x => new List<Catalog>());

            mock.Setup(m => m.GetCatalogAttachments(It.Is<int>(id => id > 4))).Returns<int>(x => new List<TextBlock>());
            mock.Setup(m => m.GetCatalogAttachments(It.Is<int>(id => id == 0)))
                .Returns<int>(x => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == 0));

            // Act

            CatalogNode expectedTree = new CatalogNode
            {
                ChildrenNodes = new List<ICatalogNode>
                {
                    new CatalogNode
                    {
                        Id = 1, Name = "Catalog1", Order = 1,
                        CatalogAttachments = new List<TextBlock>
                        {
                            new TextBlock{ Id = 2, Name = "TextBlock2", Text = "Text2", ParentCatalogId = 1, OrderInParentCatalog = 3}
                        },
                        ChildrenNodes = new List<ICatalogNode>
                        {
                            new CatalogNode
                            {
                                Id = 3, Name = "SubCatalog1", Order = 1,
                                CatalogAttachments = new List<TextBlock>
                                {
                                    new TextBlock{ Id = 3, Name = "TextBlock3", Text = "Text3", ParentCatalogId = 3, OrderInParentCatalog = 2}
                                },
                                ChildrenNodes = new List<ICatalogNode>
                                {
                                    new CatalogNode
                                    {
                                        Id = 5, Name = "SubSubCatalog1", Order = 1,
                                        ChildrenNodes = new List<ICatalogNode>()
                                    }
                                }
                            },
                            new CatalogNode
                            {
                                Id = 4, Name = "SubCatalog2", Order = 2,
                                ChildrenNodes = new List<ICatalogNode>(),
                                CatalogAttachments = new List<TextBlock>
                                {
                                    new TextBlock{ Id = 4, Name = "TextBlock4", Text = "Text4", ParentCatalogId = 4, OrderInParentCatalog = 1}
                                }
                            }
                        },
                    },
                    new CatalogNode
                    {
                        Id = 2, Name = "Catalog2", Order = 2,
                        ChildrenNodes = new List<ICatalogNode>()
                    }
                },
                CatalogAttachments = new List<TextBlock>
                {
                    new TextBlock{ Id = 1, Name = "TextBlock1", Text = "Text1", ParentCatalogId = 0, OrderInParentCatalog = 3}
                }
            };
            CatalogsTreeBuilder<CatalogNode> catalogsTreeBuilder = new CatalogsTreeBuilder<CatalogNode>(mock.Object);
            CatalogNode tree = catalogsTreeBuilder.BuildTree(0, true, true);

            // Assert

            List<ICatalogNode> expectedNodesOnCurrDepthLevel = expectedTree.ChildrenNodes;
            List<ICatalogNode> actualNodesOnCurrDepthLevel = tree.ChildrenNodes;
            while (expectedNodesOnCurrDepthLevel.Count > 0 && actualNodesOnCurrDepthLevel.Count > 0)
            {
                List<ICatalogNode> expectedNodesOnNextDepthLevel = new List<ICatalogNode>();
                List<ICatalogNode> actualNodesOnNextDepthLevel = new List<ICatalogNode>();
                Assert.Equal(expectedNodesOnCurrDepthLevel.Count, actualNodesOnCurrDepthLevel.Count);
                for (int i = 0; i < expectedNodesOnCurrDepthLevel.Count; i++)
                {
                    // Проверка равенства каталогов
                    ICatalogNode expectedCatalogNode = expectedNodesOnCurrDepthLevel[i];
                    ICatalogNode actualCatalogNode = actualNodesOnCurrDepthLevel[i];
                    expectedNodesOnNextDepthLevel.AddRange(expectedCatalogNode.ChildrenNodes);
                    actualNodesOnNextDepthLevel.AddRange(actualCatalogNode.ChildrenNodes);
                    Assert.Equal(expectedCatalogNode.Id, actualCatalogNode.Id);
                    Assert.Equal(expectedCatalogNode.Name, actualCatalogNode.Name);
                    Assert.Equal(expectedCatalogNode.Order, actualCatalogNode.Order);

                    // Проверка равенства текстовых блоков
                    List<TextBlock> expectedTextBlocks = expectedCatalogNode.CatalogAttachments;
                    List<TextBlock> actualTextBlocks = actualCatalogNode.CatalogAttachments;
                    Assert.Equal(expectedTextBlocks.Count, actualTextBlocks.Count);
                    for (int j = 0; j < expectedTextBlocks.Count; j++)
                    {
                        TextBlock currentExpectedTextBlock = expectedTextBlocks[j];
                        TextBlock currentActualTextBlock = actualTextBlocks[j];
                        Assert.Equal(currentExpectedTextBlock.Id, currentActualTextBlock.Id);
                        Assert.Equal(currentExpectedTextBlock.Name, currentActualTextBlock.Name);
                        Assert.Equal(currentExpectedTextBlock.OrderInParentCatalog, currentActualTextBlock.OrderInParentCatalog);
                        Assert.Equal(currentExpectedTextBlock.ParentCatalogId, currentActualTextBlock.ParentCatalogId);
                        Assert.Equal(currentExpectedTextBlock.Text, currentActualTextBlock.Text);
                    }
                }
                expectedNodesOnCurrDepthLevel = expectedNodesOnNextDepthLevel;
                actualNodesOnCurrDepthLevel = actualNodesOnNextDepthLevel;
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void CatalogsTreeBuilder_BuildTree_DirectoryFacadeWithOnlyTextBlocks_CorrectlyBuildedTree()
        {
            // Arrange

            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 1, Name = "TextBlock1", Text = "Text1", ParentCatalogId = 0, OrderInParentCatalog = 1},
                new TextBlock{ Id = 2, Name = "TextBlock2", Text = "Text2", ParentCatalogId = 0, OrderInParentCatalog = 2},
                new TextBlock{ Id = 3, Name = "TextBlock3", Text = "Text3", ParentCatalogId = 0, OrderInParentCatalog = 3},
                new TextBlock{ Id = 4, Name = "TextBlock4", Text = "Text4", ParentCatalogId = 0, OrderInParentCatalog = 4}
            };
            Mock<IDirectorySystemFacade> mock = new Mock<IDirectorySystemFacade>(MockBehavior.Strict);

            mock.Setup(m => m.GetCatalogById(It.IsAny<int>())).Returns<int>(x => new Catalog());
            mock.Setup(m => m.GetSubcatalogs(It.IsAny<int>())).Returns<int>(x => new List<Catalog>());

            mock.Setup(m => m.GetCatalogAttachments(It.Is<int>(id => id != 0))).Returns<int>(x => new List<TextBlock>());
            mock.Setup(m => m.GetCatalogAttachments(It.Is<int>(id => id == 0)))
                .Returns<int>(x => allTextBlocks);

            // Act

            CatalogNode expectedTree = new CatalogNode { CatalogAttachments = allTextBlocks };
            CatalogsTreeBuilder<CatalogNode> catalogsTreeBuilder = new CatalogsTreeBuilder<CatalogNode>(mock.Object);
            CatalogNode tree = catalogsTreeBuilder.BuildTree(0, true, true);

            // Assert

            Assert.Equal(expectedTree.ChildrenNodes.Count, tree.ChildrenNodes.Count);
            Assert.Equal(expectedTree.CatalogAttachments.Count, tree.CatalogAttachments.Count);
            for (int j = 0; j < expectedTree.CatalogAttachments.Count; j++)
            {
                TextBlock currentExpectedTextBlock = expectedTree.CatalogAttachments[j];
                TextBlock currentActualTextBlock = tree.CatalogAttachments[j];
                Assert.Equal(currentExpectedTextBlock.Id, currentActualTextBlock.Id);
                Assert.Equal(currentExpectedTextBlock.Name, currentActualTextBlock.Name);
                Assert.Equal(currentExpectedTextBlock.OrderInParentCatalog, currentActualTextBlock.OrderInParentCatalog);
                Assert.Equal(currentExpectedTextBlock.ParentCatalogId, currentActualTextBlock.ParentCatalogId);
                Assert.Equal(currentExpectedTextBlock.Text, currentActualTextBlock.Text);
            }
            Mock.VerifyAll();
        }
    }
}
