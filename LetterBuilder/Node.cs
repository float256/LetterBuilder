using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    public class Node
    {
        public int ElementId { get; set; }
        public NodeType Type;
        public Node ParentNode { get; private set; }
        public LinkedList<Node> ChildrenNodes { get; private set; } = new LinkedList<Node>();

        public Node(int elementId, NodeType type, Node parent = null)
        {
            ElementId = elementId;
            Type = type;
            ParentNode = parent;
        }

        public void AddChild(int elementId, NodeType type)
        {
            Node node = new Node(elementId, type, this);
            ChildrenNodes.AddLast(node);
        }

        public void AddChild(Node node)
        {
            ChildrenNodes.AddLast(node);
        }
    }

    public enum NodeType
    {
        Catalog = 1,
        TextBlock = 2
    }
}
