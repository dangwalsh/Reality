using System;
using System.Collections.Generic;
using System.Linq;

//using static Reality.ObjReader.Utils;

namespace Reality.ObjReader
{
    internal class Node : INode
    {
        public virtual string Name { get; private set; }
        public virtual INode Parent { get; private set; }
        public virtual NodeType NodeType { get; private set; }
        public virtual List<INode> Children { get; private set; }
        public virtual List<int> VertexIndex { get; private set; }
        public virtual List<int> UVIndex { get; private set; }
        public virtual List<int> NormalIndex { get; private set; }
        public virtual Material Material { get; private set; }

        public Node(INode node, string name, NodeType nodeType)
        {
            this.Parent = node;
            this.Name = name;
            this.NodeType = nodeType;
            this.Children = new List<INode>();
            this.VertexIndex = new List<int>();
            this.UVIndex = new List<int>();
            this.NormalIndex = new List<int>();
            this.Material = null;
        }

        public virtual void AddChild(string[] tokens, NodeType nodeType)
        {
            tokens = tokens.Skip(1).ToArray();
            var name = string.Join(" ", tokens);
            this.Children.Add(new Node(this, name, nodeType));
        }

        public virtual void AddMaterial(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            var name = string.Join(" ", tokens);
            var context = this.Parent as Context;
            this.Material = context.Materials
                .FirstOrDefault(m => m.Name == name);
        }

        public virtual void AddMaterial(string name)
        {
            var context = this.Parent as Context;
            this.Material = context.Materials
                .FirstOrDefault(m => m.Name == name);
        }

        public virtual void AddIndices(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            foreach (var token in tokens)
            {
                var indices = Reader.Tokenize(token, '/');
                switch (indices.Length)
                {
                    case 1:
                        if (indices[0] != "")
                            this.VertexIndex.Add(Reality.ObjReader.Utils.ConvertToInt(indices[0]) - 1);
                        break;
                    case 2:
                        if (indices[0] != "")
                            this.VertexIndex.Add(Reality.ObjReader.Utils.ConvertToInt(indices[0]) - 1);
                        if (indices[1] != "")
                            this.UVIndex.Add(Reality.ObjReader.Utils.ConvertToInt(indices[1]) - 1);
                        break;
                    case 3:
                        if (indices[0] != "")
                            this.VertexIndex.Add(Reality.ObjReader.Utils.ConvertToInt(indices[0]) - 1);
                        if (indices[1] != "")
                            this.UVIndex.Add(Reality.ObjReader.Utils.ConvertToInt(indices[1]) - 1);
                        if (indices[2] != "")
                            this.NormalIndex.Add(Reality.ObjReader.Utils.ConvertToInt(indices[2]) - 1);
                        break;
                    default:
                        throw new Exception("wrong number of indices");
                }
            }
        }
    }
}