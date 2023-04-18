using System.Diagnostics;

public class Renderer
{
    public World World { get; set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public int TargetFps { get; set; }
    public Renderer(int width, int height, int targetFps)
    {
        Width = width;
        Height = height;
        TargetFps = targetFps;
        World = new World(Width, Height);
        World.Populate();
    }

    public void Render() 
    {
        // render the world. Let the program run for exactly 30 seconds
        Stopwatch gametimer = new Stopwatch();
        gametimer.Start();
        
        // set the console properties
        // Console.SetWindowSize(Width + 20, Height + 4);
        // Console.SetBufferSize(Width + 20, Height + 4);
        Console.CursorVisible = false;

        double timeSinceLastUpdate = 0.0001; // delen door nul is nooit goed
        Thread.Sleep(10);

        double targetFrameTime = 1000.0 / TargetFps;

        bool running = true;

        while (gametimer.Elapsed.TotalSeconds < 300 && running) {
            // temp
            Console.WriteLine($"Time (via stopwatch):                  {gametimer.Elapsed.TotalMilliseconds}");
            
            // update the world while passing the time since last update;
            var tickTimer = new Stopwatch();
            tickTimer.Start();

            running = World.Update(timeSinceLastUpdate);

            // render the world
            double renderTime = RenderFrame();

            // calculate how long we need to wait to not go over
            // the target fps
            
            double timeToWait = renderTime < targetFrameTime 
                ? targetFrameTime - renderTime
                : 0;

            double frameTime = renderTime + timeToWait;
            Console.WriteLine($"fps: {(1000/frameTime)}/{TargetFps}");
            Console.WriteLine($"Rendered frame in {renderTime} ms");

            Thread.Sleep((int)timeToWait);
            timeSinceLastUpdate = (tickTimer.ElapsedMilliseconds * 0.001);
        }

        ShowEndScreen(gametimer);
    }

    public double RenderFrame() {
        // renders the world
        // return the time it took to render the frame
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        // render the world while overwriting the previous frame char by char,
        // starting at the top left of the console.
        Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
        for (int y = 0; y < World.Height; y++) {
            for (int x = 0; x < World.Width; x++) {
                Console.Write(World.GetLocation(x, y).Render_Sprite_Char());
            }
            Console.WriteLine();
        }

        return stopwatch.Elapsed.TotalMilliseconds;
    }

    public void ShowEndScreen(Stopwatch gametimer) {
        // show the end screen
        // Console.WriteLine("Game over!");
        // Console.WriteLine($"You survived for {gametimer.Elapsed.TotalSeconds} seconds");
        int totalAmountOfEntitiesLived = 0;
        int totalAmountOfPlantsLived = 0;
        int totalAmountOfRocksLived = 0;
        int totalAmountOfCrittersLived = 0;
        // var oldestCritter = World.Entities.Where(e => e is Critter).OrderByDescending(e => e.Age).FirstOrDefault();
        var oldestCritter = new Critter("temp", 0);
        foreach (var entity in World.Entities) {
            totalAmountOfEntitiesLived += 1;
            if (entity is Plant) {
                totalAmountOfPlantsLived += 1;
            } else if (entity is Rock) {
                totalAmountOfRocksLived += 1;
            } else if (entity is Critter) {
                totalAmountOfCrittersLived += 1;
            }
            if (entity.Age > oldestCritter.Age && entity is Critter) {
                oldestCritter = (Critter)entity;
            }
        }
        for (int i = 0; i < totalAmountOfCrittersLived + 1; i++) {
            Console.WriteLine();
        }
        Console.WriteLine($"You lived with {totalAmountOfEntitiesLived} entities");
        Console.WriteLine($"You lived with {totalAmountOfPlantsLived} plants");
        Console.WriteLine($"You lived with {totalAmountOfRocksLived} rocks");
        Console.WriteLine($"You lived with {totalAmountOfCrittersLived} critters");
        Console.WriteLine($"The oldest critter was {oldestCritter.Name} and it lived for {oldestCritter.Age} seconds. His stats were:");
        Console.WriteLine($"- Speed: {oldestCritter.Speed}");
        Console.WriteLine($"- EnergyCostMultiplier: {oldestCritter.EnergyCostMultiplier}");
        Console.WriteLine($"- Died by: {oldestCritter.DeathBy}");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}