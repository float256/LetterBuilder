using System;
using System.Collections.Generic;
using System.Text;

namespace LetterBuilder
{
    class Node
    {
        public int NodeData { get; set; }
        public NodeType Type;
        public Node NodeParent { get; private set; }
        public LinkedList<Node> NodeChilds { get; private set; } = new LinkedList<Node>();

        public Node(int data, NodeType type, Node parent = null)
        {
            NodeData = data;
            Type = type;
            NodeParent = parent;
        }

        public void AddChild(int data, NodeType type)
        {
            Node node = new Node(data, type, this);
            NodeChilds.AddLast(node);
        }

        public void AddChild(Node node)
        {
            NodeChilds.AddLast(node);
        }
    }

    enum NodeType
    {
        Catalog = 1,
        TextBlock = 2
    }
}
