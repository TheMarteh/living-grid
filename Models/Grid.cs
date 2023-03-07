public class World {
    public int Height { get; private set; }
    public int Width { get; private set; }
    public Location[,] Cells { get; private set; }

    public World(int height, int width) {
        Height = height;
        Width = width;
        Cells = new Location[height, width];
    }

    // fill the world with plants and rocks
    public void Populate() {
        // create a random number generator
        Random random = new Random();

        // populate the world with plants and rocks
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                // create a new location
                Cells[y, x] = new Location();

                // randomly decide whether to add a plant, rock or nothing
                int randomValue = random.Next(0, 3);
                if (randomValue == 0) {
                    // add a plant
                    Cells[y, x].ReceiveEntity(new Plant("Plant"));
                } else if (randomValue == 1) {
                    // add a rock
                    Cells[y, x].ReceiveEntity(new Rock("Rock"));
                }               
            }
        }
    }

    public Location GetLocation(int x, int y) {
        return Cells[y, x];
    }
}