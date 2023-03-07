public class Plant : Entity {
    public int Height { get; private set; }
    public Plant(string name) : base(name) { 
    }
    public void Grow() {
        // Grow the plant
        this.Height++;
    }

    public override char Render_Sprite_Char() {
        // The plant grows in the following stages:
        // 0: Seed (.)
        // 1: Sprout (,)
        // 2: Plant (Ψ)

        if (this.Height == 0) {
            return '.';
        } else if (this.Height == 1) {
            return ',';
        } else {
            return 'Ψ';
        }
    }

    public override void onSpawn() {
        // Plants start at 0 height
        this.Height = 0;

        // grow the plant randomly
        Random random = new Random();
        int randomValue = random.Next(0, 3);
        if (randomValue == 0) {
            Grow();
        }
        else if (randomValue == 1) {
            Grow();
            Grow();
        }
    }
}