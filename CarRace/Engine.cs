using System;

namespace CarRace
{
    class Engine
    {
        const float MULTIPLIER = (float)2.5;

        protected static Random randomizer = new Random();

        public bool IsActive { get; protected set; } = false;
        protected int power;

        public Engine(int power)
        {
            this.power = power;
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

            float distance = (this.power / 100) * Engine.MULTIPLIER * percentage;

            return distance > 1 ? distance : 1;
        }
    }
}