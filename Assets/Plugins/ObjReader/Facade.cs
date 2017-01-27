namespace Reality.ObjReader
{
    public static class Facade
    {
        /// <summary>
        /// Entry point for the interface.  Must be called first.
        /// </summary>
        /// <param name="filename">The full path to the file.</param>
        /// <returns>The number of objects imported.</returns>
        public static int ImportObjects(string filename)
        {
            context = null;
            return GetRoot(filename);
        }

        public static Vec3[] GetVertices()
        {
            if (context == null) return null;
            return context.Vertices.ToArray();
        }

        public static Vec3[] GetNormals()
        {
            if (context == null) return null;
            return context.Normals.ToArray();
        }

        public static Vec2[] GetUVs()
        {
            if (context == null) return null;
            return context.UVs.ToArray();
        }

        public static Vec3 GetBounds()
        {
            if (context == null) return null;
            return context.Bounds.Magnitude;
        }

        public static string GetNameOfObject(int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            if (child == null) return null;
            return child.Name;
        }

        public static int[] GetVertexIndexOfObject(int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            if (child == null) return null;
            return child.VertexIndex.ToArray();
        }

        public static int[] GetNormalIndexOfObject(int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            if (child == null) return null;
            return child.NormalIndex.ToArray();
        }

        public static int[] GetUVIndexOfObject(int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            if (child == null) return null;
            return child.UVIndex.ToArray();
        }

        public static string GetNameOfMaterialOfObject(int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            if (child == null) return null;
            return child.Material.Name;
        }

        public static float[] GetColorOfChannelOfObject(string channel, int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            switch(channel)
            {
                case "Diffuse":
                    if (child == null) return null;
                    return child.Material.Kd;
                case "Ambient":
                    if (child == null) return null;
                    return child.Material.Ka;
                case "Specular":
                    if (child == null) return null;
                    return child.Material.Ks;
                default:
                    return null;
            }
        }

        public static string GetPathOfMapOfObject(string map, int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            switch(map)
            {
                case "Diffuse":
                    if (child == null) return null;
                    return child.Material.MapKd.Path;
                case "Ambient":
                    if (child == null) return null;
                    return child.Material.MapKa.Path;
                case "Specular":
                    if (child == null) return null;
                    return child.Material.MapKs.Path;
                case "Bump":
                    if (child == null) return null;
                    return child.Material.MapBump.Path;
                default:
                    return null;
            }
        }

        public static float[] GetScaleOfMapOfObject(string map, int index)
        {
            if (context == null) return null;
            var child = context.Children[index] as Node;
            switch(map)
            {
                case "Diffuse":
                    if (child == null) return null;
                    return child.Material.MapKd.Scale;
                case "Ambient":
                    if (child == null) return null;
                    return child.Material.MapKa.Scale;
                case "Specular":
                    if (child == null) return null;
                    return child.Material.MapKs.Scale;
                case "Bump":
                    return child.Material.MapBump.Scale;
                default:
                    return null;
            }
        }

        private static int GetRoot(string filename)
        {
            var count = 0;
            context = new Reader(filename).GetRootNode() as Context;
            count = context.Children.Count;
            return count;
        }

        private static Context context;
    }
}
