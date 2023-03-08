public class World {
    public int Height { get; private set; }
    public int Width { get; private set; }
    public List<IEntity> Entities { get; private set; }
    public Location[,] Cells { get; private set; }
    public int CellSize { get; private set; }
    public Critter firstCritter { get; private set; }

    public World(int width, int height) {
        Height = height;
        Width = width;
        Entities = new List<IEntity>();
        Cells = new Location[width, height];
        CellSize = 8;
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
            this.Entities.Add(seed as Entity);
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
            this.Entities.Add((Entity)rock as Entity);
            location.ReceiveEntity(rock);
        }

        // spawn the first critter
        int critterx = random.Next(0, Width);
        int crittery = random.Next(0, Height);
        var critterLocation = Cells[critterx, crittery];
        var critter = new Critter($"FirstCritter", 13);
        this.Entities.Add(critter);
        Console.WriteLine("The critter named " + critter.Name + " has spawned!");
        critterLocation.ReceiveEntity(critter);
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
        // firstCritter.Move(dt);

        // let all movable entities move
        // for each location in the world
        // if the location has an entity
        // if the entity is movable
        // move the entity
        // if the entity has moved to a new location
        // add the entity to the new location
        // remove the entity from the old location
        for (int j = 0; j < Width; j++) {
            for (int k = 0; k < Height; k++) {
                var loc = Cells[j, k];
                if (loc.Host != null && loc.Host is IMovable) {
                    ((IMovable)loc.Host).Move(dt);
                    if (((IMovable)loc.Host).relativeXPosition > CellSize / 2) {
                        ((IMovable)loc.Host).relativeXPosition -= CellSize;
                        if (j < Width - 1) {
                            var newLoc = Cells[j + 1, k].ReceiveEntity(loc.Host);
                            loc.RemoveEntity();
                        }
                    }
                    else if (((IMovable)loc.Host).relativeXPosition < -CellSize / 2) {
                        ((IMovable)loc.Host).relativeXPosition += CellSize;
                        if (j > 0) {
                            var newLoc = Cells[j - 1, k].ReceiveEntity(loc.Host);
                            loc.RemoveEntity();
                        }
                    }
                    else if (((IMovable)loc.Host).relativeYPosition > CellSize / 2) {
                        ((IMovable)loc.Host).relativeYPosition -= CellSize;
                        if (k < Height - 1) {
                            var newLoc = Cells[j, k + 1].ReceiveEntity(loc.Host);
                            loc.RemoveEntity();
                        }
                    }
                    else if (((IMovable)loc.Host).relativeYPosition < -CellSize / 2) {
                        ((IMovable)loc.Host).relativeYPosition += CellSize;
                        if (k > 0) {
                            var newLoc = Cells[j, k - 1].ReceiveEntity(loc.Host);
                            loc.RemoveEntity();
                        }
                    }
                }
            }
        }
    }
    

    public Location GetLocation(int x, int y) {
        return Cells[x,y];
    }
}