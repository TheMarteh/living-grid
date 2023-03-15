public class Plant : Entity {
    public int Height { get; private set; }
    public double NextStageAt { get; private set; }
    public double GrowthRate { get; private set;}
    public int MaxHeight { get; private set; }
    public double EnergyGivenPerPick { get; private set;}
    public int HeightCostPerPick { get; private set;}
    public Plant(string name, double growthRate = 1.0, double energyGivenPerPick = 1.0) : base(name) { 
        HeightCostPerPick = 2;
        IsAlive = true;
        MaxHeight = 6;
        GrowthRate = growthRate;
        EnergyGivenPerPick = energyGivenPerPick;
    }
    public void Grow() {
        // Grow the plant
        if (Height < MaxHeight) this.Height++;
    }

    public override char Render_Sprite_Char() {
        // The plant grows in the following stages:
        // 0: Seed (.)
        // 1: Sprout (,)
        // 2: Seedling (ƫ)
        // 2: Plant (Ʈ)
        if (this.Height == 0) {
            return ' ';
        }
        else if (this.Height <= 2) {
            return '.';
        } 
        else if (this.Height <= 4) {
            return ',';
        } 
        else if (this.Height <= 6) {
            return '+';
        } 
        else {
            return '♠';
        }
    }

    public override void onSpawn() {
        // Plants start at 0 height and age
        this.Height = 0;
        this.Age = 0;
        NextStageAt = new Random().NextDouble() * 8.0 * GrowthRate;

        // There is a 1/4 chance to spawn as a sprout
        Random random = new Random();
        int randomValue = random.Next(0, 3);
        if (randomValue == 0) {
            Grow();
        }
    }

    public override IEntity discoverEntityOn(IEntity e, Location loc) {
        if (e is Plant) {
            GrowthRate = GrowthRate * ((Plant)e).GrowthRate * 0.95;
        }
        return null;
    }

    public override void PerformAction(double dt) {
        this.Age += dt;
        // A plant grows one stage further every 4-6 seconds
        if (this.Age > NextStageAt) {
            Grow();
            NextStageAt += new Random().NextDouble() * ( 8 * GrowthRate );
        }
    }

    public double GetEaten() {
        double givenEnergy = 0;
        if (Height >= HeightCostPerPick) {
            givenEnergy = EnergyGivenPerPick;
            Height -= HeightCostPerPick;
        }

        if (Height == 0) {
            IsAlive = false;
            Host.BuryEntity(this);
        }

        return givenEnergy;
    }
}