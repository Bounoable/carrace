namespace CarRace
{
    class Car
    {
        public string Name { get; protected set; }
        public Vector2D Position { get; protected set; }

        protected Engine engine;

        public Car(string name, Engine engine, Vector2D position = null)
        {
            this.Name = name;
            this.engine = engine;
            this.Position = position ?? new Vector2D(0, 0);
        }

        public void Drive(RaceTrack track)
        {
            this.engine.Start();

            Vector2D currentPosition = this.Position;
            float distance = this.engine.CalculateDistance();

            float newX = currentPosition.X + distance;
            int newY = currentPosition.Y;

            while (newX > track.RoundLength - 1) {
                newX -= track.RoundLength;
                newY++;
            }

            this.Position = new Vector2D(newX, newY);
        }
    }
}