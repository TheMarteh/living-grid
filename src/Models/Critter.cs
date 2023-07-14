public class Critter : Entity, IMoving
{
    public int Speed { get; set; }
    public double relativeXPosition { get; set; }
    public double relativeYPosition { get; set; }
    public double Direction { get; set; }
    public double Energy { get; private set;}
    public double EnergyCostMultiplier { get; private set;}
    public string DeathBy { get; private set;}
    public int carryTime;


    // pregnancy
    private double pregnancyTimer;
    private bool isPregnant = false;
    private CritterGenome? baby;

    public Critter(string name, int speed) : base(name)
    {
        Speed = speed;
        Host = null;
        IsAlive = true;
        Energy = 30.0;
        EnergyCostMultiplier = 1.0;
        carryTime = 5;
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
    }

    public override char Render_Sprite_Char()
    {
        return 'C';
    }

    public override IEntity discoverEntityOn(IEntity e, Location loc) {
        // Discovers an entity and decides what to do with them.
        // returns the entity, or null if it is destroyed
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
            this.DeathBy = "Smashed by a Rock";
            Host.BuryEntity(this);
            return e;
        }
        if (e is Critter) {
            // mate
            if (Energy > 10 && Age > 10) {
                // wacht even en spawn child
                Energy -=5;
                impregnate((Critter)e);
            }
        }
        // The occupant is destroyed
        return null;
    }

    public override void PerformAction(double dt)
    {
        Age += dt;
        if (!IsAlive) return;
        if (isPregnant) {
            pregnancyTimer -= dt;
            if (pregnancyTimer < 0 ) {
                // bevallen van baby
                giveBaby();
            }
        }
        Move(dt);
        if (Energy <= 0) {
            IsAlive = false;
            DeathBy = "Starved to death";
            Host.BuryEntity(this);
            return;
        }
        Host.CheckBorderCrossing(this);
    }

    public void Bounce()
    {
        Direction = new Random().NextDouble() * 2 * Math.PI;
        relativeXPosition = 0;
        relativeYPosition = 0;
    }

    public void impregnate(Critter partner) {
        // TODO: implement
        CritterGenome babyGenome = new CritterGenome();
        babyGenome.Speed_a = Speed;
        babyGenome.EnergyCostMultiplier_a = EnergyCostMultiplier;
        partner.becomePregnant(babyGenome);
    }

    public void becomePregnant(CritterGenome babyGenome) {
        this.isPregnant = true;
        this.pregnancyTimer = carryTime;
        babyGenome.Speed_b = this.Speed;
        babyGenome.EnergyCostMultiplier_b = this.EnergyCostMultiplier;
        this.baby = babyGenome;
    }

    public void giveBaby() {
        Critter baby = new Critter($"baby of {this.Name}", this.Speed);
        Host.NotifyBirth(baby);
        this.isPregnant = false;
        this.pregnancyTimer = carryTime;
        this.baby = null;
    }
}