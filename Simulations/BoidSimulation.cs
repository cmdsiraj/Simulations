/*
 * Reference: https://vanhunteradams.com/Pico/Animal_Movement/Boids-algorithm.html
 */

using Simulations.Common;

namespace Simulations
{
    enum Direction
    {
        Left,Right,Down,Up
    }

    internal struct World
    {
        public double leftMargin, rightMargin, topMargin, bottomMargin;

        public World(double left, double right, double top, double bottom)
        {
            this.leftMargin = left;
            this.rightMargin = right;
            this.topMargin = top;
            this.bottomMargin = bottom;
        }
    }
    

    internal class Boid
    {
        Vector position;
        Vector velocity;

        // TUNED PARAMETERS
        private readonly double PROTECTED_RANGE = 4.0;     
        private readonly double AVOID_FACTOR = 0.6;         
        private readonly double MATCHING_FACTOR = 0.05;     
        private readonly double CENTERING_FACTOR = 0.005;  
        private readonly double VISUAL_RANGE = 25.0;        
        private readonly double MAX_SPEED = 1.8;            
        private readonly double MIN_SPEED = 0.8;

        private readonly World world;


        public Boid(double x, double y, World world, Random random)
        {
            this.position = new Vector(x, y);

            // random direction + speed
            var angle = random.NextDouble() * Math.PI * 2.0;
            var speed = MIN_SPEED + random.NextDouble() * (MAX_SPEED - MIN_SPEED);
            var vx = Math.Cos(angle) * speed;
            var vy = Math.Sin(angle) * speed;
            this.velocity = new Vector(vx, vy);

            this.world = world;
        }


        // Each boid attempts to avoid collision into other boids.
        private Vector GetSeparation(List<Boid> neighbours)
        {
            if (neighbours == null || neighbours.Count == 0) return Vector.Zero;
            Vector steer = Vector.Zero;
            foreach (var other in neighbours)
            {
                var distance = Vector.Distance(this.position, other.position);
                if (distance < PROTECTED_RANGE && distance > 0)
                {
                    
                    steer += (this.position - other.position) / (distance * distance);
                }
            }
            return steer * AVOID_FACTOR;
        }

        // Each boid attempts to match the velocity of other boids inside its visible range.
        private Vector GetAlignment(List<Boid> neighbours)
        {
            if (neighbours == null || neighbours.Count == 0) return Vector.Zero;
            Vector avg_v = Vector.Zero;
            foreach (var boid in neighbours)
            {
                avg_v += boid.velocity;
            }
            avg_v /= neighbours.Count;
            return (avg_v - this.velocity) * MATCHING_FACTOR;
        }

        // Each boid steers gently towards the center of mass of other boids within its visible range
        private Vector GetCohesion(List<Boid> neighbours)
        {
            if (neighbours == null || neighbours.Count == 0) return Vector.Zero;
            Vector avg_d = Vector.Zero;
            foreach (var boid in neighbours)
            {
                avg_d += boid.position;
            }
            avg_d /= neighbours.Count;
            return (avg_d - this.position) * CENTERING_FACTOR;
        }

        private void ClampSpeed()
        {
            var speed = this.velocity.Magnitude;
            if (speed > MAX_SPEED)
            {
                this.velocity = this.velocity.Normalize * MAX_SPEED;
            }
            else if (speed < MIN_SPEED)
            {
                this.velocity = (speed == 0) ? new Vector(MIN_SPEED, 0) : this.velocity.Normalize * MIN_SPEED;
            }
        }

        public void Update(List<Boid> boids)
        {
            var neighbours = boids.Where(b => !ReferenceEquals(this, b) && Vector.Distance(this.position, b.position) < VISUAL_RANGE).ToList();
            this.velocity += GetSeparation(neighbours) + GetAlignment(neighbours) + GetCohesion(neighbours);

            this.AdjustInFrame();

            this.position += this.velocity;
        }

        private void AdjustInFrame()
        {
            // bounce: clamp position and reflect velocity
            if (this.position.X < world.leftMargin)
            {
                this.position.X = world.leftMargin;
                this.velocity.X = Math.Abs(this.velocity.X);
            }
            else if (this.position.X > world.rightMargin)
            {
                this.position.X = world.rightMargin;
                this.velocity.X = -Math.Abs(this.velocity.X);
            }

            if (this.position.Y < world.topMargin)
            {
                this.position.Y = world.topMargin;
                this.velocity.Y = Math.Abs(this.velocity.Y);
            }
            else if (this.position.Y > world.bottomMargin)
            {
                this.position.Y = world.bottomMargin;
                this.velocity.Y = -Math.Abs(this.velocity.Y);
            }

            ClampSpeed();
        }

        public Vector GetPosition()
        {
            return this.position;
        }

        public Vector GetVelocity()
        {
            return this.velocity;
        }

    }
    internal class BoidSimulation
    {
        private List<Boid> boids;
        private readonly World world;
        private readonly Random rand = new Random();
        private readonly string boidRep;

        public BoidSimulation(string boidRep = ">", int flokSize = 50, double leftMargin=40, double rightMargin=90, double topMargin=5, double bottomMargin=25)
        {
            boids = new List<Boid>();
            this.boidRep = boidRep;

            world = new World(leftMargin, rightMargin, topMargin, bottomMargin);


            for (int i = 0; i < flokSize; i++)
            {
                int x = (int)(leftMargin + (rand.NextDouble() * (rightMargin - leftMargin)));
                int y = (int)(topMargin + (rand.NextDouble() * (bottomMargin - topMargin)));

                boids.Add(new Boid(x, y, this.world, rand));
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
                    Console.Write(this.boidRep);
                }
                Thread.Sleep(100);
            }
            
        }

    }
}
