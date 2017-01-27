using System;

namespace Reality.ObjReader
{
    public class Vec4
    {
        public Vec4()
        {
            this.data = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
        }
        
        public Vec4(float f1, float f2, float f3, float f4)
        {
            this.data = new float[] { f1, f2, f3, f4 };
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
                    data = new float[4];
                data[key] = value;
            }
        }

        public static implicit operator float[](Vec4 rhs)
        {
            return new float[] {
                rhs[0], rhs[1], rhs[2], rhs[3]};
        }

        public static float[] operator +(Vec4 lhs, float rhs)
        {
            return new float[]{lhs[0], lhs[1], lhs[2], lhs[3], rhs};
        }

        public static Vec4 operator -(Vec4 lhs, Vec4 rhs)
        {
            return new Vec4(
                lhs[0] - rhs[0], 
                lhs[1] - rhs[1], 
                lhs[2] - rhs[2], 
                lhs[3] - rhs[3]
                );
        }

        public static implicit operator Vec4(float[] rhs)
        {
            return new Vec4 (rhs[0], rhs[1], rhs[2], rhs[3]);
        }

        private float[] data { get; set; }
    }
}