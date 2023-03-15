public class World {
    public int Height { get; private set; }
    public int Width { get; private set; }
    public List<IEntity> Entities { get; private set; }
    public Location[,] Cells { get; private set; }
    public int CellSize { get; private set; }
    public Critter firstCritter { get; private set; }
    public double timer { get; private set; }

    public World(int width, int height) {
        Height = height;
        Width = width;
        Entities = new List<IEntity>();
        Cells = new Location[width, height];
        CellSize = 8;
        for (int x=0;x<width;x++) {
            for (int y = 0; y< height; y++) {
                Cells[x,y] = new Location(this, x, y);
            }
        }
    }

    // fill the world with plants and rocks
    public void Populate() {
        // create a random number generator
        Random random = new Random();

        
        // grow a few random plants
        for (int i = 0; i < 70; i++) {
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
        for (int i = 0; i < 3; i++ ) {
            // get a random location
            int x = random.Next(0, Width);
            int y = random.Next(0, Height);
            var location = Cells[x, y];

            // put the rock on
            var rock = new Rock($"stonehenge_{i}");
            this.Entities.Add((Entity)rock as Entity);
            location.ReceiveEntity(rock);
        }

        
        // spawn some critters
        for (int i = 0; i < 4; i++) {
            int critterx = new Random().Next(0, Width);
            int crittery = new Random().Next(0, Height);
            var critterLocation = Cells[critterx, crittery];
            var critter = new Critter($"Critter {i}", 40);
            this.Entities.Add(critter);
            Console.WriteLine("The critter named " + critter.Name + " has spawned!");
            critterLocation.ReceiveEntity(critter);
        }
        

        /* FOR DEBUGGING
        int critterx = 3;
        int crittery = 3;
        var critterLocation = Cells[critterx, crittery];
        var critter = new Critter($"Critter {1}", 40);
        critter.Direction = 0;
        this.Entities.Add(critter);
        Console.WriteLine("The critter named " + critter.Name + " has spawned!");
        critterLocation.ReceiveEntity(critter);

        int x = 15;
        int y = 3;
        var location = Cells[x,y];

        // plant a seed in that location
        var seed = new Plant($"gepotte_plant_{1}");
        this.Entities.Add(seed as Entity);
        Cells[x,y].ReceiveEntity(seed);
        */
    }
    

    public bool Update(double dt) {
        // temp
        timer += dt;
        Console.WriteLine("Time (via counting dt)                 " + timer );

        // update the world
        // dt is the time in milliseconds since the last update

        // For now, pick a random plant and grow it once.
        // DONT DO THIS!!
        // update using the dt variable instead.
        // We can't know how often this function is called. The
        // dt variable tells us the time delta since last update, so we can use that to 
        // add simulation behaviour
        
        // Random random = new Random();
        // int x = random.Next(0, Width);
        // int y = random.Next(0, Height);
        // var location = Cells[x, y];
        // if (location.Host != null && location.Host is Plant) {
        //     var plant = (Plant)location.Host;
        //     plant.Grow();
        // }
        
        // Do this instead:
        bool SomeAreStillAlive = false;
        foreach (var entity in Entities) {
            if (entity.IsAlive) {
                entity.PerformAction(dt);
                if (entity is Critter) SomeAreStillAlive = true;
            }
            if (entity is Critter) {
                Console.WriteLine("The critter named " + ((Critter)entity).Name + " has " + ((Critter)entity).Energy + " health left.");
            }
        }

        return SomeAreStillAlive;
    }

    public Entity MoveEntity(Location oldLoc, IMovable e) {
        // zoek waar de entity heen moet

        int newLocX = oldLoc.X;
        int newLocY = oldLoc.Y;

        if (e.relativeXPosition - CellSize / 2 > 0) {
            e.relativeXPosition =1 - CellSize / 2;
            newLocX++;
        }
        if (e.relativeXPosition + CellSize / 2 < 0) {
            e.relativeXPosition= CellSize / 2 - 1;
            newLocX--;
        }

        if (e.relativeYPosition - CellSize / 2 > 0) {
            e.relativeYPosition = 1 - CellSize / 2;
            newLocY++;
        }
        if (e.relativeYPosition + CellSize / 2 < 0) {
            e.relativeYPosition = CellSize / 2 - 1;
            newLocY--;
        }

        if (newLocX < 0  || newLocX >= Width || newLocY < 0 || newLocY >= Height) {
            // de nieuwe locatie is buiten de wereld, we geven de entity terug zodat de locatie
            // weet dat hij hem niet heeft weg kunnen geven
            return e as Entity;
        }

        // verplaats de entity naar de nieuwe locatie
        var newLoc = Cells[newLocX, newLocY];
        var entity = newLoc.ReceiveEntity(e as IEntity);
        
        // todo, checken wat te doen met een teruggegeven entity
        return null;


    }
    

    public Location GetLocation(int x, int y) {
        return Cells[x,y];
    }
}