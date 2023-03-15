public class Critter : Entity, IMovable
{
    public int Speed { get; set; }
    public double relativeXPosition { get; set; }
    public double relativeYPosition { get; set; }
    public double Direction { get; set; }
    public double Energy { get; private set;}
    public double EnergyCostMultiplier { get; private set;}

    public Critter(string name, int speed) : base(name)
    {
        Speed = speed;
        Host = null;
        IsAlive = true;
        Energy = 30.0;
        EnergyCostMultiplier = 0.7;
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
        Energy -= EnergyCostMultiplier * dt * 1;
        if (Energy <= 0) {
            IsAlive = false;
            Host.BuryEntity(this);
        }
    }

    public override char Render_Sprite_Char()
    {
        return 'C';
    }

    public override IEntity discoverEntityOn(IEntity e, Location loc) {
        Host = loc;
        if (e is Plant) {
            // TODO: Eat plant, but give it back
            var gainedEnergy = ((Plant)e).GetEaten();
            Energy += gainedEnergy;
            return e.IsAlive ? e : null;
        }
        if (e is Rock) {
            // TODO: Die..
            this.IsAlive = false;
            Host.BuryEntity(this);
            return e;
        }
        // The occupant is destroyed
        return null;
    }

    public override void PerformAction(double dt)
    {
        Age += dt;
        if (!IsAlive) return;
        Move(dt);
        Host.CheckBorderCrossing(this);
    }

    public void Bounce()
    {
        Direction = new Random().NextDouble() * 2 * Math.PI;
        relativeXPosition = 0;
        relativeYPosition = 0;
    }
}