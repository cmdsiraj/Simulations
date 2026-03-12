using System;
using System.Collections.Generic;
using System.Text;
using Simulations.Common.Structs;

/*Reference: https://vanhunteradams.com/Pico/Animal_Movement/Boids-algorithm.html*/

namespace Simulations.SimulationBoid
{
    internal class BoidSimulation
    {
        private List<Boid> boids;
        private readonly World world;
        private readonly Random rand = new Random();
        private readonly BoidConfig config;

        public BoidSimulation(int flokSize = 50, double leftMargin = 40, double rightMargin = 90, double topMargin = 5, double bottomMargin = 25)
        {
            boids = new List<Boid>();
            this.config = new BoidConfig();

            world = new World(leftMargin, rightMargin, topMargin, bottomMargin);


            for (int i = 0; i < flokSize; i++)
            {
                int x = (int)(leftMargin + (rand.NextDouble() * (rightMargin - leftMargin)));
                int y = (int)(topMargin + (rand.NextDouble() * (bottomMargin - topMargin)));

                boids.Add(new Boid(x, y, this.world, this.config, rand));
            }
        }

        public void Simulate()
        {
            Console.CursorVisible = false;
            while (true)
            {
                Console.Clear();
                foreach (var boid in boids)
                {
                    boid.Update(boids);

                    var pos = boid.GetPosition();
                    int drawX = (int)Math.Clamp(pos.X, this.world.leftMargin, this.world.rightMargin - 1);
                    int drawY = (int)Math.Clamp(pos.Y, this.world.topMargin, this.world.bottomMargin - 1);

                    drawX = Math.Clamp(drawX, 0, Console.BufferWidth - 1);
                    drawY = Math.Clamp(drawY, 0, Console.BufferHeight - 1);

                    Console.SetCursorPosition(drawX, drawY);
                    Console.Write(this.config.BoidRep);
                }
                Thread.Sleep(100);
            }

        }

    }
}
