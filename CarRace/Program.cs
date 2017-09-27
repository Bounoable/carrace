using System;
using System.Text;
using System.Threading;

namespace CarRace
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Wie soll die Strecke heißen?");
            string trackName = Console.ReadLine();

            Console.Clear();
            Console.WriteLine("Wie viele Runden werden gefahren?");
            
            int rounds = 0;

            while (!Int32.TryParse(Console.ReadLine(), out rounds)) {
                Console.Clear();
                Console.WriteLine("Ungültige Rundenzahl angegeben. Nochmal!");
            }

            Console.Clear();
            Console.WriteLine("Wie lang ist eine Runde?");

            int roundLength = 0;

            while (!Int32.TryParse(Console.ReadLine(), out roundLength)) {
                Console.Clear();
                Console.WriteLine("Ungültige Rundenlänge angegeben. Nochmal!");
            }


            Console.Clear();
            Console.WriteLine("Wie viele Autos treten an?");

            int carCount = 0;

            while (!Int32.TryParse(Console.ReadLine(), out carCount) || carCount == 0) {
                Console.Clear();
                Console.WriteLine("Ungültige Zahl. Nochmal!");
            }

            Car[] cars = new Car[carCount];

            for (int i = 0; i < cars.Length; ++i) {
                Console.Clear();
                Console.WriteLine("Wie soll das " + (i + 1) + ". Auto heißen?");
                string name = Console.ReadLine();
                
                Console.WriteLine("Welche Power hat das Auto?");

                int power = 0;

                while (!Int32.TryParse(Console.ReadLine(), out power)) {
                    Console.Clear();
                    Console.WriteLine("Ungültige Power angegeben (muss von Typ int sein). Nochmal!");
                }

                cars[i] = new Car(name, new Engine(power));
            }

            RaceTrack track = new RaceTrack(trackName, rounds, roundLength);
            Race race = new Race(track, cars);

            race.Run();
        }
    }

    class Engine
    {
        const float MULTIPLIER = (float)2.5;

        protected static Random randomizer = new Random();

        public bool IsActive { get; protected set; } = false;
        public int Power { get; protected set; }

        public Engine(int power)
        {
            this.Power = power;
        }

        public void Start()
        {
            if (Engine.randomizer.Next(1, 6) == 1) {
                this.IsActive = false;
                return;
            }

            this.IsActive = true;
        }

        public float CalculateDistance()
        {
            if (!this.IsActive) return 0;

            float percentage = Engine.randomizer.Next(50, 101) / 100;

            return (this.Power / 100) * Engine.MULTIPLIER * percentage;
        }
    }

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

            float newX = (currentPosition.X + distance);
            int newY = currentPosition.Y;

            while (newX > track.RoundLength - 1) {
                newX = newX - track.RoundLength;
                newY++;
            }

            this.Position = new Vector2D(newX, newY);
        }
    }

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

    class Race
    {
        const int COUNTDOWN = 3;

        protected RaceTrack track;
        protected Car[] cars;
        protected bool isFinished = false;

        public Car[] Winners { get; protected set; } = new Car[0];

        public Race(RaceTrack track, Car[] cars)
        {
            this.track = track;
            this.cars = cars;
        }

        public void Run()
        {
            this.Introduce();
            this.DrawRaceTrack();

            while (!this.isFinished) {
                this.GiveDriveCommand();
                this.DrawRaceTrack();
                this.CheckForFinish();
                Thread.Sleep(2);
            }

            this.RevealWinners();
        }

        protected void Introduce()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("~~~~~~~~~~~~~~~ ")
              .Append(this.track.Name)
              .Append(" ~~~~~~~~~~~~~~~\n")
              .Append("Runden: ")
              .Append(this.track.Rounds)
              .Append(" | Rundenlänge: ")
              .Append(this.track.RoundLength)
              .Append(" | Teilnehmer: ")
              .Append(this.cars.Length)
              .Append("\n\n");

            Console.Clear();
            Console.WriteLine(sb.ToString());

            for (int i = Race.COUNTDOWN; i > 0; --i) {
                sb = new StringBuilder();

                sb.Append($"Das Rennen beginnt in {i}...");
                Console.WriteLine(sb.ToString());
                Thread.Sleep(1000);
            }
        }

        protected void GiveDriveCommand()
        {
            foreach (Car car in this.cars) {
                car.Drive(this.track);
            }
        }

        protected void DrawRaceTrack()
        {
            Console.Clear();
            StringBuilder sb = new StringBuilder();

            for (int y = 0; y < this.track.Rounds; ++y) {
                sb.Append(this.GetRoundView(y + 1));
            }

            Console.WriteLine(sb.ToString());
        }

        protected string GetRoundView(int round)
        {
            StringBuilder sb = new StringBuilder();
            int y = round - 1;

            for (int x = 0; x < this.track.RoundLength; ++x) {
                Vector2D position = new Vector2D(x, y);

                if (this.AnyCarIsAtPosition(position)) {
                    sb.Append(this.GetCarView(position));
                    continue;
                }

                sb.Append("- ");
            }

            return sb.Append("\n").ToString();
        }

        protected bool AnyCarIsAtPosition(Vector2D position)
        {
            return this.GetCarsAtPosition(position).Length > 0;
        }

        protected Car[] GetCarsAtPosition(Vector2D position)
        {
            return Array.FindAll(this.cars, car => this.CarIsAtPosition(car, position));
        }

        protected bool CarIsAtPosition(Car car, Vector2D position) {
            int carX = (int)Math.Round(car.Position.X);
            int carY = car.Position.Y;

            return carX == position.X && carY == position.Y;
        }

        protected string GetCarView(Vector2D position)
        {
            Car[] cars = this.GetCarsAtPosition(position);
            StringBuilder sb = new StringBuilder();

            return sb.Append(cars.Length > 1 ? "O" : cars[0].Name[0].ToString()).Append(" ").ToString();
        }

        protected void CheckForFinish()
        {
            Car[] cars = this.GetFinishedCars();

            if (cars.Length == 0) return;

            this.Winners = cars;
            this.isFinished = true;
        }

        protected Car[] GetFinishedCars()
        {
            return Array.FindAll(this.cars, car => this.CarIsFinished(car));
        }

        protected bool CarIsFinished(Car car)
        {
            return car.Position.Y > this.track.Rounds - 1;
        }

        protected void RevealWinners()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Das Rennen ist beendet!\n")
              .Append(this.Winners.Length == 1 ? "Der Sieger ist " : "Dies Sieger sind ");
            
            string[] winners = new string[this.Winners.Length];

            for (int i = 0; i < this.Winners.Length; ++i) {
                winners[i] = this.Winners[i].Name;
            }

            sb.Append(string.Join(" & ", winners));

            Console.Clear();
            Console.WriteLine(sb.ToString());
        }
    }
}
