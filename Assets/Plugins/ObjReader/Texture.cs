using System.Linq;

namespace Reality.ObjReader
{
    internal class Texture
    {
        public Vec3 Scale { get; private set; }
        public Vec3 Origin { get; private set; }
        public Vec3 Turbulence { get; private set; }
        public string Path { get; private set; }

        public Texture()
        {
            this.Scale = new Vec3(1.0f, 1.0f, 1.0f);
            this.Origin = new Vec3();
            this.Turbulence = new Vec3();
            this.Path = null;
        }
        
        public Texture(string[] tokens)
        {
            this.Scale = new Vec3(1.0f, 1.0f, 1.0f);
            this.Origin = new Vec3();
            this.Turbulence = new Vec3();
            this.Path = null;

            while (tokens != null)
                ProcessFlags(ref tokens);
        }

        protected void ProcessFlags(ref string[] tokens)
        {
            switch (tokens[0])
            {
                case "-s":
                    tokens = tokens.Skip(1).ToArray();
                    this.Scale = Utils.ConvertToVec3(tokens);
                    tokens = tokens.Skip(3).ToArray();
                    break;
                case "-o":
                    tokens = tokens.Skip(1).ToArray();
                    this.Origin = Utils.ConvertToVec3(tokens);
                    tokens = tokens.Skip(3).ToArray();
                    break;
                case "-t":
                    tokens = tokens.Skip(1).ToArray();
                    this.Turbulence = Utils.ConvertToVec3(tokens);
                    tokens = tokens.Skip(3).ToArray();
                    break;
                default:
                    this.Path = Utils.ConvertToString(tokens);
                    tokens = null;
                    break;
            }
        }
    }
}