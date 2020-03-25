using System;
using System.Collections.Generic;
using Xunit;
using LetterBuilder;

namespace LetterBuilder.Tests
{
    public class NodeTests
    {
        [Fact]
        public void Node_AddElementId_NodeWithCorrectElementId()
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
        public void Node_AddElementId_NodeWithCorrectType()
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
        public void EmptyTree_AddSomeChildren_nonEmptyTreeWithCorrectChildrenElementId()
        {
            // Arrange
            Node node = new Node(0, NodeType.Catalog);

            // Act
            List<int> testValues = new List<int>();
            Random randomGenerator = new Random();
            for (int i = 0; i < randomGenerator.Next(1, 100); i++)
            {
                testValues.Add(randomGenerator.Next());
                node.AddChild(testValues[i], NodeType.Catalog);
            }

            // Assert
            List<int> valuesFromTree = new List<int>();
            foreach (Node item in node.ChildrenNodes)
            {
                valuesFromTree.Add(item.ElementId);
            }
            Assert.Equal(testValues, valuesFromTree);
        }

        [Fact]
        public void EmptyTree_AddSomeChildren_nonEmptyTreeWithCorrectChildrenType()
        {
            // Arrange
            Node node = new Node(0, NodeType.Catalog);

            // Act
            List<NodeType> testTypes = new List<NodeType>();
            Random randomGenerator = new Random();
            for (int i = 0; i < randomGenerator.Next(1, 100); i++)
            {
                testTypes.Add((NodeType) randomGenerator.Next(1, 3));
                node.AddChild(0, testTypes[i]);
            }

            // Assert
            List<NodeType> typesFromTree = new List<NodeType>();
            foreach (Node item in node.ChildrenNodes)
            {
                typesFromTree.Add(item.Type);
            }
            Assert.Equal(testTypes, typesFromTree);
        }
    }
}
