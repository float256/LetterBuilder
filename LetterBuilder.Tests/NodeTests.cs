using System;
using System.Collections.Generic;
using Xunit;
using LetterBuilder;

namespace LetterBuilder.Tests
{
    public class NodeTests
    {
        [Fact]
        public void Node_SetElementId_NodeWithoutChildren_NodeWithCorrectElementId()
        {
            // Arrange
            Node node = new Node(0, NodeType.Catalog);

            // Act
            Random random = new Random();
            int testElementId = random.Next();
            node.ElementId = testElementId;

            // Assert
            Assert.Equal(testElementId, node.ElementId);
        }

        [Fact]
        public void Node_SetType_NodeWithoutChildren_NodeWithCorrectType()
        {
            // Arrange
            Node node = new Node(0, NodeType.Catalog);

            // Act
            Random random = new Random();
            NodeType testType = (NodeType) random.Next(1, 3);
            node.Type = testType;

            // Assert
            Assert.Equal(testType, node.Type);
        }

        [Fact]
        public void Node_AddChild_FiveChildrenNodes_AllChildrenNodesAddedWithCorrectElementIdAndType()
        {
            // Arrange
            Node node = new Node(0, NodeType.Catalog);
            List<int> testElementsId = new List<int> { 30239, 0 , 1, 123, 837293};
            List<NodeType> testTypes = new List<NodeType>()
            {
                NodeType.Catalog,
                NodeType.TextBlock,
                NodeType.Catalog,
                NodeType.TextBlock,
                NodeType.Catalog
            };

            // Act
            for (int i = 0; i < 5; i++)
            {
                node.AddChild(testElementsId[i], testTypes[i]);
            }

            // Assert
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(testElementsId[i], node.ChildrenNodes[i].ElementId);
                Assert.Equal(testTypes[i], node.ChildrenNodes[i].Type);
            }
        }

        [Fact]
        public void Node_AddChild_FiveChildrenNodes_AllChildrenNodesAddedInCorrectOrder()
        {
            // Arrange
            Node node = new Node(0, NodeType.Catalog);
            List<Node> testNodes = new List<Node>
            {
                new Node(2, NodeType.Catalog),
                new Node(3189, NodeType.TextBlock),
                new Node(1, NodeType.TextBlock),
                new Node(223, NodeType.Catalog),
                new Node(1111, NodeType.Catalog)
            };

            // Act
            foreach (Node item in testNodes)
            {
                node.AddChild(item);
            }

            // Assert
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(node.ChildrenNodes[i], testNodes[i]);
            }
        }
    }
}
