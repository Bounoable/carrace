using System;

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

            while (!Int32.TryParse(Console.ReadLine(), out carCount) || carCount <= 1) {
                Console.Clear();
                Console.WriteLine("Ungültige Zahl. Nochmal!");
            }

            Car[] cars = new Car[carCount];

            for (int i = 0; i < cars.Length; ++i) {
                Console.Clear();
                Console.WriteLine($"Wie soll das {i + 1}. Auto heißen?");
                string name = Console.ReadLine();
                
                Console.WriteLine("Wie viel PS hat das Auto?");

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
}
