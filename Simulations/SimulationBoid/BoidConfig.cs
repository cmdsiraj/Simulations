namespace Simulations.SimulationBoid
{
    internal sealed class BoidConfig
    {
        public double ProtectedRange { get; }
        public double AvoidFactor { get; }
        public double MatchingFactor { get; }
        public double CenteringFactor { get; }
        public double VisualRange { get; }
        public double MaxSpeed { get; }
        public double MinSpeed { get; }
        public int SleepMs { get; }
        public string BoidRep { get; }

        public BoidConfig(
            double protectedRange = 4.0,
            double avoidFactor = 0.6,
            double matchingFactor = 0.05,
            double centeringFactor = 0.005,
            double visualRange = 25.0,
            double maxSpeed = 1.8,
            double minSpeed = 0.8,
            int sleepMs = 100,
            string boidRep = ">"
        )
        {
            ProtectedRange = protectedRange;
            AvoidFactor = avoidFactor;
            MatchingFactor = matchingFactor;
            CenteringFactor = centeringFactor;
            VisualRange = visualRange;
            MaxSpeed = maxSpeed;
            MinSpeed = minSpeed;
            SleepMs = sleepMs;
            BoidRep = boidRep;
        }
    }
}
