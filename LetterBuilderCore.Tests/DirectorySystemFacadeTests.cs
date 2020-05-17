using System;
using Xunit;
using Moq;
using LetterBuilderCore.Services;
using LetterBuilderCore.Services.DAO;
using LetterBuilderCore.Models;
using System.Collections.Generic;

namespace LetterBuilderCore.Tests
{
    public class DirectorySystemFacadeTests
    {
        [Fact]
        public void DirectorySystemFacade_AddCatalog_AddCatalogInNotEmptyRepository_CorrectlyAddedCatalog()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 0 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0},
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 7, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.Add(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Add(catalog));

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));

            // Act
            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<Catalog> expectedCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 0 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100 },
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100 },
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0},
                new Catalog{ Id = 101, Name = "Catalog", OrderInParentCatalog = 8, ParentCatalogId = 100 }
            };
            Catalog catalogForAdding = new Catalog
            {
                Id = 101,
                Name = "Catalog",
                OrderInParentCatalog = 8,
                ParentCatalogId = 100
            };
            directorySystemFacade.Add(catalogForAdding);

            // Assert
            Assert.Equal(expectedCatalogs.Count, allCatalogs.Count);
            for (int i = 0; i < expectedCatalogs.Count; i++)
            {
                AssertCatalog(expectedCatalogs[i], allCatalogs[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_AddTextBlock_AddTextBlockInNotEmptyRepository_CorrectlyAddedTextBlock()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 0 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0},
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 7, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.Add(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Add(textBlock));
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));

            // Act
            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<TextBlock> expectedTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 7, ParentCatalogId = 100, Text = "Text3"},
                new TextBlock{ Id = 8, Name = "TextBlock4", OrderInParentCatalog = 8, ParentCatalogId = 100, Text = "Text"}
            };
            TextBlock textBlockForAdding = new TextBlock
            {
                Id = 8,
                Name = "TextBlock4",
                ParentCatalogId = 100,
                Text = "Text"
            };
            directorySystemFacade.Add(textBlockForAdding);

            // Assert
            Assert.Equal(expectedTextBlocks.Count, allTextBlocks.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertTextBlock(expectedTextBlocks[i], allTextBlocks[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_UpdateParentCatalogOfCatalog_NotEmptyCatalogAndTextBlockRepositories_CorrectlyUpdatedCatalog()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 7, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).OrderInParentCatalog = catalog.OrderInParentCatalog);
            catalogDataAccessMock.Setup(m => m.UpdateParentCatalog(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).ParentCatalogId = catalog.ParentCatalogId);

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));
            textDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).OrderInParentCatalog = textBlock.OrderInParentCatalog);
            textDataAccessMock.Setup(m => m.UpdateParentCatalog(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).ParentCatalogId = textBlock.ParentCatalogId);

            // Act

            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<Catalog> expectedCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 8, ParentCatalogId = 100 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            Catalog catalogForUpdating = new Catalog
            {
                Id = 1,
                ParentCatalogId = 100,
            };
            directorySystemFacade.UpdateParentCatalog(catalogForUpdating);

            // Assert

            Assert.Equal(expectedCatalogs.Count, allCatalogs.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertCatalog(expectedCatalogs[i], allCatalogs[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_UpdateParentCatalogOfTextBlock_NotEmptyCatalogAndTextBlockRepositories_CorrectlyUpdatedTextBlock()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).OrderInParentCatalog = catalog.OrderInParentCatalog);
            catalogDataAccessMock.Setup(m => m.UpdateParentCatalog(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).ParentCatalogId = catalog.ParentCatalogId);

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));
            textDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).OrderInParentCatalog = textBlock.OrderInParentCatalog);
            textDataAccessMock.Setup(m => m.UpdateParentCatalog(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).ParentCatalogId = textBlock.ParentCatalogId);

            // Act

            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<TextBlock> exprectedTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 7, ParentCatalogId = 100, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            TextBlock textBlockForUpdating = new TextBlock
            {
                Id = 2,
                ParentCatalogId = 100,
            };
            directorySystemFacade.UpdateParentCatalog(textBlockForUpdating);

            // Assert

            Assert.Equal(exprectedTextBlocks.Count, allTextBlocks.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertTextBlock(exprectedTextBlocks[i], allTextBlocks[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_UpdateOrderOfTextBlock_MovingUpTextBlock_CorrectlyUpdatedTextBlock()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).OrderInParentCatalog = catalog.OrderInParentCatalog);

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));
            textDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).OrderInParentCatalog = textBlock.OrderInParentCatalog);

            // Act

            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<Catalog> expectedCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 3, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> exprectedTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 2, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            TextBlock textBlockForReordering = new TextBlock
            {
                Id = 4,
                ParentCatalogId = 100,
                OrderInParentCatalog = 3
            };
            directorySystemFacade.UpdateOrder(textBlockForReordering, OrderAction.MoveUp);

            // Assert

            Assert.Equal(exprectedTextBlocks.Count, allTextBlocks.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertTextBlock(exprectedTextBlocks[i], allTextBlocks[i]);
            }
            Assert.Equal(expectedCatalogs.Count, allCatalogs.Count);
            for (int i = 0; i < allCatalogs.Count; i++)
            {
                AssertCatalog(expectedCatalogs[i], allCatalogs[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_UpdateOrderOfTextBlock_MovingDownTextBlock_CorrectlyUpdatedTextBlock()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).OrderInParentCatalog = catalog.OrderInParentCatalog);

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));
            textDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).OrderInParentCatalog = textBlock.OrderInParentCatalog);

            // Act

            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<Catalog> expectedCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 3, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> exprectedTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 4, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            TextBlock textBlockForReordering = new TextBlock
            {
                Id = 4,
                ParentCatalogId = 100,
                OrderInParentCatalog = 3
            };
            directorySystemFacade.UpdateOrder(textBlockForReordering, OrderAction.MoveDown);

            // Assert

            Assert.Equal(exprectedTextBlocks.Count, allTextBlocks.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertTextBlock(exprectedTextBlocks[i], allTextBlocks[i]);
            }
            Assert.Equal(expectedCatalogs.Count, allCatalogs.Count);
            for (int i = 0; i < allCatalogs.Count; i++)
            {
                AssertCatalog(expectedCatalogs[i], allCatalogs[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_UpdateOrderOfCatalog_MovingUpCatalog_CorrectlyUpdatedCatalog()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).OrderInParentCatalog = catalog.OrderInParentCatalog);

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));
            textDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).OrderInParentCatalog = textBlock.OrderInParentCatalog);

            // Act

            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<Catalog> expectedCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 3, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> exprectedTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 4, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            Catalog catalogForReordering = new Catalog
            {
                Id = 3,
                ParentCatalogId = 100,
                OrderInParentCatalog = 4
            };
            directorySystemFacade.UpdateOrder(catalogForReordering, OrderAction.MoveUp);

            // Assert

            Assert.Equal(exprectedTextBlocks.Count, allTextBlocks.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertTextBlock(exprectedTextBlocks[i], allTextBlocks[i]);
            }
            Assert.Equal(expectedCatalogs.Count, allCatalogs.Count);
            for (int i = 0; i < allCatalogs.Count; i++)
            {
                AssertCatalog(expectedCatalogs[i], allCatalogs[i]);
            }
            Mock.VerifyAll();
        }

        [Fact]
        public void DirectorySystemFacade_UpdateOrderOfCatalog_MovingDownCatalog_CorrectlyUpdatedCatalog()
        {
            // Arrange

            List<Catalog> allCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 4, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> allTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 6, ParentCatalogId = 100, Text = "Text3"}
            };
            Mock<ICatalogDataAccess> catalogDataAccessMock = new Mock<ICatalogDataAccess>(MockBehavior.Strict);
            catalogDataAccessMock.Setup(m => m.GetSubcatalogsByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allCatalogs.FindAll(catalog => catalog.ParentCatalogId == id));
            catalogDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<Catalog>()))
                .Callback((Catalog catalog) => allCatalogs.Find(x => x.Id == catalog.Id).OrderInParentCatalog = catalog.OrderInParentCatalog);

            Mock<ITextBlockDataAccess> textDataAccessMock = new Mock<ITextBlockDataAccess>();
            textDataAccessMock.Setup(m => m.GetTextBlocksByParentCatalogId(It.IsAny<int>()))
                .Returns<int>(id => allTextBlocks.FindAll(textBlock => textBlock.ParentCatalogId == id));
            textDataAccessMock.Setup(m => m.UpdateOrder(It.IsAny<TextBlock>()))
                .Callback((TextBlock textBlock) => allTextBlocks.Find(x => x.Id == textBlock.Id).OrderInParentCatalog = textBlock.OrderInParentCatalog);

            // Act

            DirectorySystemFacade directorySystemFacade = new DirectorySystemFacade(textDataAccessMock.Object, catalogDataAccessMock.Object);
            List<Catalog> expectedCatalogs = new List<Catalog>
            {
                new Catalog{ Id = 1, Name = "Catalog1", OrderInParentCatalog = 1, ParentCatalogId = 4 },
                new Catalog{ Id = 3, Name = "Catalog2", OrderInParentCatalog = 6, ParentCatalogId = 100},
                new Catalog{ Id = 4, Name = "Catalog3", OrderInParentCatalog = 2, ParentCatalogId = 100},
                new Catalog{ Id = 100, Name = "Catalog100", OrderInParentCatalog = 2, ParentCatalogId = 0}
            };
            List<TextBlock> exprectedTextBlocks = new List<TextBlock>
            {
                new TextBlock{ Id = 2, Name = "TextBlock1", OrderInParentCatalog = 1, ParentCatalogId = 0, Text = "Text1" },
                new TextBlock{ Id = 4, Name = "TextBlock2", OrderInParentCatalog = 3, ParentCatalogId = 100, Text = "Text2"},
                new TextBlock{ Id = 7, Name = "TextBlock3", OrderInParentCatalog = 4, ParentCatalogId = 100, Text = "Text3"}
            };
            Catalog catalogForReordering = new Catalog
            {
                Id = 3,
                ParentCatalogId = 100,
                OrderInParentCatalog = 4
            };
            directorySystemFacade.UpdateOrder(catalogForReordering, OrderAction.MoveDown);

            // Assert

            Assert.Equal(exprectedTextBlocks.Count, allTextBlocks.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertTextBlock(exprectedTextBlocks[i], allTextBlocks[i]);
            }
            Assert.Equal(expectedCatalogs.Count, allCatalogs.Count);
            for (int i = 0; i < allTextBlocks.Count; i++)
            {
                AssertCatalog(expectedCatalogs[i], allCatalogs[i]);
            }
            Mock.VerifyAll();
        }

        private void AssertTextBlock(TextBlock expectedTextBlock, TextBlock actualTextBlock)
        {
            Assert.Equal(expectedTextBlock.Id, actualTextBlock.Id);
            Assert.Equal(expectedTextBlock.Name, actualTextBlock.Name);
            Assert.Equal(expectedTextBlock.Text, actualTextBlock.Text);
            Assert.Equal(expectedTextBlock.OrderInParentCatalog, actualTextBlock.OrderInParentCatalog);
            Assert.Equal(expectedTextBlock.ParentCatalogId, actualTextBlock.ParentCatalogId);
        }

        private void AssertCatalog(Catalog expectedCatalog, Catalog actualCatalog)
        {
            Assert.Equal(expectedCatalog.Id, actualCatalog.Id);
            Assert.Equal(expectedCatalog.Name, actualCatalog.Name);
            Assert.Equal(expectedCatalog.OrderInParentCatalog, actualCatalog.OrderInParentCatalog);
            Assert.Equal(expectedCatalog.ParentCatalogId, actualCatalog.ParentCatalogId);
        }
    }
}
