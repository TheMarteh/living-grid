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
                    var plant = new Plant("Plant");
                    Cells[y, x].ReceiveEntity(plant);
                } else if (randomValue == 1) {
                    // add a rock
                    Cells[y, x].ReceiveEntity(new Rock("Rock"));
                }               
            }
        }

        // grow a few random plants
        for (int i = 0; i < 10; i++) {
            // get a random location
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);

            // get the entity at that location
            var entity = Cells[y, x].Host;

            // if there is a plant, grow it
            if (entity is Plant) {
                var plant = (Plant)entity;
                plant.Grow();
            }
        }
    }

    public Location GetLocation(int x, int y) {
        return Cells[y, x];
    }
}