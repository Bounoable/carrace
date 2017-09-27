using System;
using System.Text;
using System.Threading;

namespace CarRace
{
    class Race
    {
        const int COUNTDOWN = 3;

        protected RaceTrack track;
        protected Car[] cars;

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

            while (!this.IsFinished()) {
                this.GiveDriveCommand();
                this.DrawRaceTrack();
                this.CheckForFinish();
                Thread.Sleep(100);
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
                Console.WriteLine($"Das Rennen beginnt in {i}...");
                Thread.Sleep(1000);
            }
        }

        protected bool IsFinished()
        {
            return this.Winners.Length > 0;
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

                sb.Append(this.GetPointView(position));
            }

            return sb.Append("\n").ToString();
        }

        protected string GetPointView(Vector2D position)
        {
            if (position.X == 0 && position.Y == 0) {
                return "| ";
            }

            if (position.X == this.track.RoundLength - 1 && position.Y == this.track.Rounds - 1) {
                return "|";
            }

            return "- ";
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
        }

        protected Car[] GetFinishedCars()
        {
            return Array.FindAll(this.cars, car => this.CarIsFinished(car));
        }

        protected bool CarIsFinished(Car car)
        {
            return (car.Position.Y > this.track.Rounds - 1)
                || (car.Position.X == this.track.RoundLength - 1 && car.Position.Y == this.track.Rounds - 1);
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