using System;
using System.Text;
using System.Threading;

namespace CarRace
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Car[] cars = new Car[2];
            Car carA = new Car("A", new Engine((float)1.5));
            Car carB = new Car("B", new Engine((float)2.5));
            
            cars[0] = carA;
            cars[1] = carB;

            RaceTrack track = new RaceTrack();
            Race race = new Race(track, cars);

            race.Run();
        }
    }

    class Engine
    {
        const int MULTIPLIER = 5;

        protected static Random randomizer = new Random();

        public bool IsActive { get; protected set; }
        public float Power { get; protected set; }

        public Engine(float power)
        {
            this.Power = power;
        }

        public void Start()
        {
            if (Engine.randomizer.Next(1, 5) == 1) return;

            this.IsActive = true;
        }

        public void Stop()
        {
            this.IsActive = false;
        }

        public float CalculateDistance()
        {
            float percentage = Engine.randomizer.Next(50, 101) / 100;

            return this.Power * Engine.MULTIPLIER * percentage;
        }
    }

    class Vector2D
    {
        public float X { get; protected set; }
        public float Y { get; protected set; }

        public Vector2D(float x = 0, float y = 0)
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

            if (! this.engine.IsActive) return;

            Vector2D currentPosition = this.Position;
            float distance = this.engine.CalculateDistance();

            float newX = currentPosition.X + distance;
            float newY = currentPosition.Y;

            while (Math.Round(newX) > track.RoundLength - 1) {
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

        public RaceTrack(string name = "Circuit City", int rounds = 10, int roundLength = 30)
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

        public Car[] Winners { get; protected set; }

        public Race(RaceTrack track, Car[] cars)
        {
            this.track = track;
            this.cars = cars;
        }

        public void Run()
        {
            this.Introduce();
            this.DrawRaceTrack();

            while (! this.isFinished) {
                this.GiveDriveCommand();
                this.DrawRaceTrack();
                this.CheckForFinish();
                Thread.Sleep(50);
            }
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
              .Append("\n\n");

            Console.Clear();
            Console.WriteLine(sb.ToString());

            for (int i = Race.COUNTDOWN; i > 0; --i) {
                sb = new StringBuilder();

                sb.Append("Das Rennen beginnt in ").Append(i).Append("...");
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
            StringBuilder sb = new StringBuilder();

            for (int round = 1; round <= this.track.Rounds; ++round) {
                sb.Append(this.GetRoundView(round));
            }

            Console.Clear();
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

                sb.Append("= ");
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
            float carX = (float)Math.Round(car.Position.X);
            float carY = (float)Math.Round(car.Position.Y);

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
            Car[] cars = this.GetCarsPassedPosition(new Vector2D(this.track.RoundLength - 1, this.track.Rounds - 1));

            if (cars.Length == 0) return;

            this.Winners = cars;
            this.isFinished = true;
        }

        protected Car[] GetCarsPassedPosition(Vector2D position)
        {
            return Array.FindAll(this.cars, car => this.CarHasPassedPosition(car, position));
        }

        protected bool CarHasPassedPosition(Car car, Vector2D position)
        {
            return car.Position.X >= position.X && car.Position.Y >= position.Y;
        }
    }
}
