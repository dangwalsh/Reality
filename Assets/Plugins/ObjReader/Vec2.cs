using System;

namespace Reality.ObjReader
{
    public class Vec2
    {
        public Vec2()
        {
            data = new float[2] { 0.0f, 0.0f };
        }

        public Vec2(float f1, float f2)
        {
            this.data = new float[] { f1, f2 };
        }

        public float this[int key]
        {
            get
            {
                if (data == null)
                    throw new Exception("array must be initialized");
                return data[key];
            }
            set
            {
                if (data == null)
                    data = new float[2];
                data[key] = value;
            }
        }

        public static float[] operator +(Vec2 lhs, float rhs)
        {
            return new float[] { lhs[0], lhs[1], rhs };
        }

        public static Vec2 operator -(Vec2 lhs, Vec2 rhs)
        {
            return new Vec2(
                lhs[0] - rhs[0], 
                lhs[1] - rhs[1]
                );
        }

        public static implicit operator float[] (Vec2 rhs)
        {
            return new float[] { rhs[0], rhs[1] };
        }

        public static implicit operator Vec2(float[] rhs)
        {
            return new Vec2(rhs[0], rhs[1]);
        }

        private float[] data { get; set; }
    }
}