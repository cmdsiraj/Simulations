using Simulations.Common;
using Simulations.Common.Structs;
using Simulations.Common.Enums;

namespace Simulations.SimulationBoid
{
    internal class Boid
    {
        Vector position;
        Vector velocity;

        private readonly BoidConfig config;

        private readonly World world;


        public Boid(double x, double y, World world, BoidConfig config, Random random)
        {
            this.position = new Vector(x, y);
            this.config = config;


            // random direction + speed
            var angle = random.NextDouble() * Math.PI * 2.0;
            var speed = this.config.MinSpeed + random.NextDouble() * (this.config.MaxSpeed - this.config.MinSpeed);
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
                if (distance < this.config.ProtectedRange && distance > 0)
                {

                    steer += (this.position - other.position) / (distance * distance);
                }
            }
            return steer * this.config.AvoidFactor;
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
            return (avg_v - this.velocity) * this.config.MatchingFactor;
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
            return (avg_d - this.position) * this.config.CenteringFactor;
        }

        private void ClampSpeed()
        {
            var speed = this.velocity.Magnitude;
            if (speed > this.config.MaxSpeed)
            {
                this.velocity = this.velocity.Normalize * this.config.MaxSpeed;
            }
            else if (speed < this.config.MinSpeed)
            {
                this.velocity = (speed == 0) ? new Vector(this.config.MinSpeed, 0) : this.velocity.Normalize * this.config.MinSpeed;
            }
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

        public void Update(List<Boid> boids)
        {
            var neighbours = boids.Where(b => !ReferenceEquals(this, b) && Vector.Distance(this.position, b.position) < this.config.VisualRange).ToList();
            this.velocity += GetSeparation(neighbours) + GetAlignment(neighbours) + GetCohesion(neighbours);

            this.AdjustInFrame();

            this.position += this.velocity;
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
}
