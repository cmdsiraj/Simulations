using System;
using Simulations.SimulationBoid;

namespace Simulations
{
    internal class Progrme
    {
        static void Main(string[] args)
        {
            int width = Console.WindowWidth, height=Console.WindowHeight;
            double leftMargin = 10, rightMargin = width - 10;
            double topMargin = 2, bottomMargin = height - 2;

            var simulator = new BoidSimulation(flokSize: 50, leftMargin: leftMargin, rightMargin: rightMargin, topMargin: topMargin, bottomMargin: bottomMargin);
            simulator.Simulate();
        }
    }
}