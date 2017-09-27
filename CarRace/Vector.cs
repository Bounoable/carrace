namespace CarRace
{
    class Vector2D
    {
        public float X { get; protected set; }
        public int Y { get; protected set; }

        public Vector2D(float x = 0, int y = 0)
        {
            this.X = x;
            this.Y = y;
        }
    }
}