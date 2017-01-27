namespace Reality.ObjReader
{
    public struct Bounds
    {
        public Vec3 Max;
        public Vec3 Min;
        public Vec3 Magnitude;

        public Bounds(Vec3 min, Vec3 max)
        {
            this.Max = max;
            this.Min = min;
            this.Magnitude = max - min;
        }
    }
}
