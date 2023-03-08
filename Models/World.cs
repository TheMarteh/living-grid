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
        for (int i = 0; i < 25; i++) {
            // select a random location
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            var location = Cells[x,y];

            // plant a seed in that location
            var seed = new Plant($"gepotte_plant_{i}");
            Cells[x,y].ReceiveEntity(seed);
        }

        // spawn a few rocks
        for (int i = 0; i < 5; i++ ) {
            // get a random location
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            var location = Cells[x, y];

            // put the rock on
            var rock = new Rock($"stonehenge_{i}");
            location.ReceiveEntity(rock);
        }
    }

    public void Update(double dt) {
        // update the world
        // dt is the time in milliseconds since the last update

        // For now, pick a random plant and grow it once.
        // DONT DO THIS!!
        // update using the dt variable instead.
        // We can't know how often this function is called. The
        // dt variable tells us the time delta since last update, so we can use that to 
        // add simulation behaviour
        
        Random random = new Random();
        int x = random.Next(0, Width);
        int y = random.Next(0, Height);
        var location = Cells[x, y];
        if (location.Host != null && location.Host is Plant) {
            var plant = (Plant)location.Host;
            plant.Grow();
        }
        
        // Do this instead:


    }

    public Location GetLocation(int x, int y) {
        return Cells[x,y];
    }
}