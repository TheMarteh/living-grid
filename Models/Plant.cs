public class Plant : Entity {
    public int Height { get; private set; }
    public Plant(string name) : base(name) { 
        Height = 0;
    }
    private void Grow() {
        // Grow the plant
        Height++;
    }

    public override char Render() {
        // The plant grows in the following stages:
        // 0: Seed (.)
        // 1: Sprout (,)
        // 2: Plant (Ψ)

        if (Height == 0) {
            return '.';
        } else if (Height == 1) {
            return ',';
        } else {
            return 'Ψ';
        }
    }
}