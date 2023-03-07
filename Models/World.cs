public class World {
    public int Height { get; private set; }
    public int Width { get; private set; }
    public Location[,] Cells { get; private set; }

    public World(int width, int height) {
        Height = height;
        Width = width;
        Cells = new Location[width, height];
        for (int x=0;x<width;x++) {
            for (int y = 0; y< height; y++) {
                Cells[x,y] = new Location();
            }
        }
    }

    // fill the world with plants and rocks
    public void Populate() {
        // create a random number generator
        Random random = new Random();

        // grow a few random plants
        for (int i = 0; i < 10; i++) {
            // select a random location
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            var location = Cells[x,y];

            // plant a seed in that location
            var seed = new Plant($"gepotte_plant_{i}");
            Cells[x,y].ReceiveEntity(seed);
        }

        // spawn a few rocks
        for (int i = 0; i < 10; i++ ) {
            // get a random location
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            var location = Cells[x, y];

            // put the rock on
            var rock = new Rock($"stonehenge_{i}");
            location.ReceiveEntity(rock);
        }
    }

    public Location GetLocation(int x, int y) {
        return Cells[x,y];
    }
}