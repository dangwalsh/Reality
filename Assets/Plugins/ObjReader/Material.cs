using System.Linq;

//using static Reality.ObjReader.Utils;

namespace Reality.ObjReader
{
    internal class Material
    {
        /// <summary>
        /// Owner of the material.
        /// </summary>
        public INode Parent { get; private set; }

        /// <summary>
        /// Name of the material.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Diffuse color value (rgba).
        /// </summary>
        public Vec4 Kd { get; private set; }

        /// <summary>
        /// Ambient color value (rgba).
        /// </summary>
        public Vec4 Ka { get; private set; }

        /// <summary>
        /// Specular color value (rgba).
        /// </summary>
        public Vec4 Ks { get; private set; }

        /// <summary>
        /// Specular exponent (focus of the hilight).
        /// </summary>
        public float Ns { get; private set; }

        /// <summary>
        /// Diffuse texture map.
        /// </summary>
        public Texture MapKd { get; private set; }

        /// <summary>
        /// Ambient texture map.
        /// </summary>
        public Texture MapKa { get; private set; }

        /// <summary>
        /// Specular texture map.
        /// </summary>
        public Texture MapKs { get; private set; }

        /// <summary>
        /// Bump texture map.
        /// </summary>
        public Texture MapBump { get; private set; }

        public Material(INode parent, string name)
        {
            this.Parent = parent;
            this.Name = name;
            this.Kd = new Vec4();
            this.Ka = new Vec4();
            this.Ks = new Vec4();
            this.Ns = 0.0f;
            this.MapKd = new Texture();
            this.MapKa = new Texture();
            this.MapKs = new Texture();
            this.MapBump = new Texture();
        }

        public void AddKd(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.Kd = Reality.ObjReader.Utils.ConvertToVec3(tokens) + 1.0f;
        }

        public void AddKa(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.Ka = Reality.ObjReader.Utils.ConvertToVec3(tokens) + 1.0f;
        }

        public void AddKs(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.Ks = Reality.ObjReader.Utils.ConvertToVec3(tokens) + 1.0f;
        }

        public void AddNs(string[] tokens)
        {
            this.Ns = Reality.ObjReader.Utils.ConvertToFloat(tokens[1]);
        }

        public void AddD(string[] tokens)
        {
            this.Kd[3] = Reality.ObjReader.Utils.ConvertToFloat(tokens[1]);
        }

        public void AddKdMap(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.MapKd = new Texture(tokens);
        }

        public void AddKaMap(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.MapKa = new Texture(tokens);
        }

        public void AddKsMap(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.MapKs = new Texture(tokens);
        }

        public void AddBumpMap(string[] tokens)
        {
            tokens = tokens.Skip(1).ToArray();
            this.MapBump = new Texture(tokens);
        }
    }
}