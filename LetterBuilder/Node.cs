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
        public List<Node> ChildrenNodes { get; private set; } = new List<Node>();

        public Node(int elementId, NodeType type, Node parent = null)
        {
            ElementId = elementId;
            Type = type;
            ParentNode = parent;
        }

        public void AddChild(int elementId, NodeType type)
        {
            Node node = new Node(elementId, type, this);
            ChildrenNodes.Add(node);
        }

        public void AddChild(Node node)
        {
            ChildrenNodes.Add(node);
        }
    }
}
