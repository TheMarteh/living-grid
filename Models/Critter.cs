public class Critter : Entity, IMovable
{
    public int Speed { get; set; }
    public double relativeXPosition { get; set; }
    public double relativeYPosition { get; set; }
    public double Direction { get; set; }
    public bool IsAlive { get; set; }

    public Critter(string name, int speed) : base(name)
    {
        Speed = speed;
        Host = null;
        IsAlive = true;
    }
    public override void onSpawn()
    {
        relativeXPosition = 0;
        relativeYPosition = 0;
        double randomDirection = new Random().NextDouble() * 2 * Math.PI;
        Direction = randomDirection;
    }
    public void Move(double dt)
    {
        relativeXPosition += (Speed * dt * Math.Cos(Direction % (2 * Math.PI)));
        relativeYPosition += (Speed * dt * Math.Sin(Direction % (2 + Math.PI)));
    }

    public override char Render_Sprite_Char()
    {
        return 'C';
    }

    public override IEntity discoverEntityOn(IEntity e, Location loc) {
        Host = loc;
        if (e is Plant) {
            return this;
        }
        if (e is Rock) {
            this.IsAlive = false;
            return e;
        }
        return e;
    }

    public override void PerformAction(double dt)
    {
        if (!IsAlive) return;
        Move(dt);
        Host.CheckBorderCrossing(relativeXPosition, relativeYPosition);
    }

    public void Bounce()
    {
        Direction = new Random().NextDouble() * 2 * Math.PI;
        relativeXPosition = 0;
        relativeYPosition = 0;
    }
}