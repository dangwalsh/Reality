using System;

namespace Reality.ObjReader
{
    public class Vec3
    {
        public Vec3()
        {
            this.data = new float[3] { 0.0f, 0.0f, 0.0f };
        }
        
        public Vec3(float f1, float f2, float f3)
        {
            this.data = new float[] { f1, f2, f3 };
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
                    data = new float[3];
                data[key] = value;
            }
        }

        public static float[] operator +(Vec3 lhs, float rhs)
        {
            return new float[]{lhs[0], lhs[1], lhs[2], rhs};
        }

        public static Vec3 operator -(Vec3 lhs, Vec3 rhs)
        {
            return new Vec3(
                lhs[0] - rhs[0], 
                lhs[1] - rhs[1], 
                lhs[2] - rhs[2]
                );
        }

        public static implicit operator float[] (Vec3 rhs)
        {
            return new float[] { rhs[0], rhs[1], rhs[2] };
        }

        public static implicit operator Vec3(float[] rhs)
        {
            return new Vec3 (rhs[0], rhs[1], rhs[2]);
        }


        
        private float[] data { get; set; }
    }
}