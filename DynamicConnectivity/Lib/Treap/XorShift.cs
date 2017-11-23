namespace DynamicConnectivity.Lib
{
    public static class XorShift
    {
        static uint x = 123456789, y = 362436069, z = 521288629, w = 88675123;

        public static uint Get()
        {
            uint t = (x ^ (x << 11));
            x = y;
            y = z;
            z = w;
            return (w = (w ^ (w >> 19)) ^ (t ^ (t >> 8)));
        }
    }
}
