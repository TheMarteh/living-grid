public class Plant : Entity {
    public int Height { get; private set; }
    public double Age { get; private set; }
    public double GrowthRate { get; private set; }
    public double EnergyGivenPerPick { get; private set;}
    public int HeightCostPerPick { get; private set;}
    public Plant(string name) : base(name) { 
        HeightCostPerPick = 2;
        IsAlive = true;
    }
    public void Grow() {
        // Grow the plant
        this.Height++;
    }

    public override char Render_Sprite_Char() {
        // The plant grows in the following stages:
        // 0: Seed (.)
        // 1: Sprout (,)
        // 2: Seedling (ƫ)
        // 2: Plant (Ʈ)

        if (this.Height == 0) {
            return '.';
        } 
        else if (this.Height == 1) {
            return ',';
        } 
        else if (this.Height == 2) {
            return 'ɂ';
        } 
        else if (this.Height == 3) {
            return 'Y';
        } 
        else {
            return 'Ȳ';
        }
    }

    public override void onSpawn() {
        // Plants start at 0 height and age
        this.Height = 0;
        this.Age = 0;
        GrowthRate = new Random().NextDouble() * 2 + 4;

        // There is a 1/4 chance to spawn as a sprout
        Random random = new Random();
        int randomValue = random.Next(0, 3);
        if (randomValue == 0) {
            Grow();
        }
    }

    public override IEntity discoverEntityOn(IEntity e, Location loc) {
        this.Host = loc;
        return this;
    }

    public override void PerformAction(double dt) {
        this.Age += dt;
        // A plant grows one stage further every 4-6 seconds
        if (this.Age > GrowthRate) {
            Grow();
            GrowthRate += new Random().NextDouble() * 2 + 4;
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
        }

        return givenEnergy;
    }
}