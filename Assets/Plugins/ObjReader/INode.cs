using System.Collections.Generic;

namespace Reality.ObjReader
{
    internal interface INode
    {
        string Name { get; }
        INode Parent { get; }
        List<INode> Children { get; }
        NodeType NodeType { get; }
        void AddChild(string[] tokens, NodeType nodeType);
    }
}