using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reality.ObjReader
{
    internal static class Utils
    {
        public static Vec3 ConvertToVec3(string[] tokens)
        {
            var vector = new Vec3();
            for (int i = 0; i < 3; i++)
            {
                float f;
                if (!float.TryParse(tokens[i], out f))
                {
                    throw new Exception("failed to convert vector3");
                }
                vector[i] = f;
            }
            return vector;
        }

        public static Vec2 ConvertToVec2(string[] tokens)
        {
            var vector = new Vec2();
            for (int i = 0; i < 2; i++)
            {
                float f;
                if (!float.TryParse(tokens[i], out f))
                {
                    throw new Exception("failed to convert vector3");
                }
                vector[i] = f;
            }
            return vector;
        }

        public static float ConvertToFloat(string token)
        {
            float val = 0;
            if (!float.TryParse(token, out val))
            {
                throw new Exception("failed to convert float");
            }
            return val;
        }

        public static int ConvertToInt(string token)
        {
            int i = 0;
            if (!int.TryParse(token, out i))
            {
                throw new Exception("failed to convert int");
            }
            return i;
        }

        public static string ConvertToString(string[] tokens)
        {
            return string.Join(" ", tokens).Trim();
        }

        public static Bounds CenterVertices(ref List<Vec3> vertices)
        {
            float max_X = -1 * float.MaxValue;
            float max_Y = -1 * float.MaxValue;
            float max_Z = -1 * float.MaxValue;

            float min_X = float.MaxValue;
            float min_Y = float.MaxValue;
            float min_Z = float.MaxValue;

            float mid_X = 0;
            float mid_Y = 0;
            float mid_Z = 0;

            for (int i = 0; i < vertices.Count; ++i)
            {
                Vec3 vertex = vertices[i];

                float x = vertex[0];
                float y = vertex[1];
                float z = vertex[2];

                if (x > max_X) max_X = x;
                if (y > max_Y) max_Y = y;
                if (z > max_Z) max_Z = z;

                if (x < min_X) min_X = x;
                if (y < min_Y) min_Y = y;
                if (z < min_Z) min_Z = z;
            }

            mid_X = (max_X + min_X) / 2;
            mid_Y = (max_Y + min_Y) / 2;
            mid_Z = (max_Z + min_Z) / 2;

            for (int i = 0; i < vertices.Count; ++i)
            {
                Vec3 vertex = vertices[i];

                float x = vertex[0];
                float y = vertex[1];
                float z = vertex[2];

                //vertex[0] = (mid_X < 0) ? (x - mid_X) : (x - mid_X);
                //vertex[1] = (mid_Y < 0) ? (y - mid_Y) : (y - mid_Y);
                //vertex[2] = (mid_Z < 0) ? (z - mid_Z) : (z - mid_Z);
                vertex[0] = x - mid_X;
                vertex[1] = y - mid_Y;
                vertex[2] = z - mid_Z;
            }

            var max = new Vec3(
                max_X - mid_X,
                max_Y - mid_Y,
                max_Z - mid_Z
                );

            var min = new Vec3(
                min_X - mid_X,
                min_Y - mid_Y,
                min_Z - mid_Z
                );

            return new Bounds(min, max);
        }

        public static float GetUnitVector(List<Vec3> vertices)
        {
            float max_A = 0;
            for (int i = 0; i < vertices.Count; ++i)
            {
                Vec3 vertex = vertices[i];

                float x = vertex[0];
                float y = vertex[1];
                float z = vertex[2];

                float a = (float)Math.Sqrt((x * x) + (y * y) + (z * z));
                if (a > max_A) max_A = a;
            }
            return max_A;
        }

        public static void NormalizeVertices(float unit, List<Vec3> vertices)
        {
            for (int i = 0; i < vertices.Count; ++i)
            {
                Vec3 vertex = vertices[i];

                float x = vertex[0];
                float y = vertex[1];
                float z = vertex[2];

                x = x / unit;
                y = y / unit;
                z = z / unit;
            }
        }

        public static string ExtractDirectory(string filename)
        {
            var directory = Path.GetDirectoryName(filename);
            return directory;
        }
    }
}