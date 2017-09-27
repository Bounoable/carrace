namespace CarRace
{
    class RaceTrack
    {
        public string Name { get; protected set; }
        public int Rounds { get; protected set; }
        public int RoundLength { get; protected set; }

        public RaceTrack(string name = "Circuit City", int rounds = 10, int roundLength = 60)
        {
            this.Name = name;
            this.Rounds = rounds;
            this.RoundLength = roundLength;
        }
    }
}