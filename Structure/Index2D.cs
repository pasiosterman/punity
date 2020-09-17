
namespace PUnity
{
    [System.Serializable]
    public struct Index2D
    {
        public int x;
        public int y;

        public Index2D(int inX, int inY)
        {
            x = inX;
            y = inY;
        }

        public Index2D RightIndex { get { return new Index2D(x + 1, y); } }
        public Index2D LeftIndex { get { return new Index2D(x - 1, y); } }
        public Index2D UpIndex { get { return new Index2D(x, y + 1); } }
        public Index2D DownIndex { get { return new Index2D(x, y - 1); } }

        public override bool Equals(object obj)
        {
            if (!(obj is Index2D)) return false;

            Index2D p = (Index2D)obj;
            return x == p.x & y == p.y;
        }

        public override int GetHashCode()
        {
            var calculation = x ^ y;
            return calculation.GetHashCode();
        }

        public static bool operator ==(Index2D c1, Index2D c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Index2D c1, Index2D c2)
        {
            return !c1.Equals(c2);
        }

        public static Index2D Zero { get { return new Index2D(0, 0); } }
    }
}