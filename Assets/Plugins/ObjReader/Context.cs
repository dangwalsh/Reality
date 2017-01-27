using System;
using System.Collections.Generic;
using System.Linq;

//using static Reality.ObjReader.Utils;

namespace Reality.ObjReader
{
    internal class Context : INode
    {
        public virtual List<INode> Children { get; set; }
        public virtual string Name { get; set; }
        public virtual NodeType NodeType { get; private set; }
        public virtual INode Parent { get; private set; }
        public virtual List<Vec2> UVs { get; private set; }
        public virtual List<Vec3> Normals { get; private set; }
        public virtual List<Material> Materials { get; private set; }
        public virtual Bounds Bounds { get; private set; }
        public static Context GetNewContext
        {
            get { return new Context(); }
        }

        public virtual List<Vec3> Vertices
        {
            get { return vertices; }
            private set { vertices = value; }
        }

        public virtual string MaterialLibrary
        {
            get { return materialLibrary; }
            private set
            {
                if (materialLibrary != null)
                    throw new Exception("multiple material libraries defined");
                materialLibrary = value;
                BuildMaterials(Reader.Read(materialLibrary));
            }

        }

        public Context()
        {
            this.Name = "Root";
            this.NodeType = NodeType.Root;
            this.Parent = null;
            this.Children = new List<INode>();
            this.Vertices = new List<Vec3>();
            this.UVs = new List<Vec2>();
            this.Normals = new List<Vec3>();
            this.Materials = new List<Material>();
        }

        public virtual void AddChild(string[] tokens, NodeType nodeType)
        {
            tokens = tokens.Skip(1).ToArray();
            var name = string.Join(" ", tokens);
            this.Children.Add(new Node(this, name, nodeType));
        }

        public virtual void AddVertex(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            var point = Reality.ObjReader.Utils.ConvertToVec3(tokens);
            this.Vertices.Add(point);
        }

        public virtual void AddUV(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            var point = Reality.ObjReader.Utils.ConvertToVec2(tokens);
            this.UVs.Add(point);
        }

        public virtual void AddNormal(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            var point = Reality.ObjReader.Utils.ConvertToVec3(tokens);
            this.Normals.Add(point);
        }

        public virtual void Center()
        {
            this.Bounds = Reality.ObjReader.Utils.CenterVertices(ref vertices);
        }

        public virtual void AddMaterialLibrary(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.MaterialLibrary = string.Join(" ", tokens);
        }

        public virtual void AddMaterials(List<string> materials)
        {
            for (int i = 0; i < this.Children.Count; ++i)
            {
                var node = this.Children[i] as Node;
                node.AddMaterial(materials[i]);
            }
        }

        protected virtual Material AddMaterial(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            var name = string.Join(" ", tokens);
            var material = this.Materials
                .FirstOrDefault(m => m.Name == name);
            if (material == null)
            {
                material = new Material(this, name);
                this.Materials.Add(material);
            }
            return material;
        }

        protected virtual void BuildMaterials(string[] lines)
        {
            Material current = null;
            foreach (var line in lines)
            {
                var tokens = Reader.Tokenize(line as string);
                switch (tokens[0])
                {
                    case "newmtl":
                        current = this.AddMaterial(tokens);
                        break;
                    case "Kd":
                        if (current == null) break;
                        current.AddKd(tokens);
                        break;
                    case "Ka":
                        if (current == null) break;
                        current.AddKa(tokens);
                        break;
                    case "Ks":
                        if (current == null) break;
                        current.AddKs(tokens);
                        break;
                    case "map_Kd":
                        if (current == null) break;
                        current.AddKdMap(tokens);
                        break;
                    case "map_Ka":
                        if (current == null) break;
                        current.AddKaMap(tokens);
                        break;
                    case "map_Ks":
                        if (current == null) break;
                        current.AddKsMap(tokens);
                        break;
                    case "map_bump":
                        if (current == null) break;
                        current.AddBumpMap(tokens);
                        break;
                    case "d":
                        if(current == null) break;
                        current.AddD(tokens);
                        break;
                    case "Ns":
                        if (current == null) break;
                        current.AddNs(tokens);
                        break;
                    default:
                        break;
                }
            }
        }

        private string materialLibrary;
        private List<Vec3> vertices;
    }
}