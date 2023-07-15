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

    public void StartOutput(int maxWidth, int maxHeight) 
    {
        // render the world. Let the program run for exactly 30 seconds
        Stopwatch gametimer = new Stopwatch();
        gametimer.Start();

        int amountOfLinesForStats = maxHeight - World.Height - 4;
        
        // set the console properties
        // Console.SetWindowSize(Width + 20, Height + 4);
        // Console.SetBufferSize(Width + 20, Height + 4);
        Console.Clear();
        Console.CursorVisible = false;
        Console.Write($"x{String.Concat(Enumerable.Repeat("-", maxWidth))}x\n");

        double timeSinceLastUpdate = 0.0001; // delen door nul is nooit goed
        Thread.Sleep(10);

        double targetFrameTime = 1000.0 / TargetFps;

        bool running = true;
        var tickTimer = new Stopwatch();

        // stop altijd na 5 minuten
        while (gametimer.Elapsed.TotalSeconds < 300 && running) {
            
            // update the world while passing the time since last update;
            tickTimer.Reset();
            tickTimer.Start();

            running = World.Update(timeSinceLastUpdate);

            // render the world
            Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
            string frameAsString = RenderFrame();
            Console.Write(frameAsString);


            // calculate how long we need to wait to not go over
            // the target fps
            
            double timeToWait = tickTimer.Elapsed.TotalMilliseconds < targetFrameTime 
                ? targetFrameTime - tickTimer.Elapsed.TotalMilliseconds
                : 0;

            double frameTime = tickTimer.Elapsed.TotalMilliseconds + timeToWait;
            // Console.WriteLine($"fps: {(Math.Round(1000/frameTime,2))}/{TargetFps}");
            // Console.WriteLine($"Rendered frame in {renderTime} ms");

            // render stats
            string stats = RenderStats(amountOfLinesForStats);
            Console.Write(stats);
            Thread.Sleep((int)timeToWait);
            timeSinceLastUpdate = (tickTimer.ElapsedMilliseconds * 0.001);
        }

        ShowEndScreen(gametimer);
    }

    private string RenderStats(int maxLines)
    {
        string result = "";
        int i = 0;
        
        foreach (IEntity e in World.Entities) {
            if (e is Critter && i < maxLines) {
                    result += ("The critter named " + ((Critter)e).Name + " Energy: " + Math.Round(((Critter)e).Energy,2)+ " State: " + ((Critter)e).DeathBy + "\n");
                    i++;
                }
        }
        
        return result;
    }

    public string RenderFrame() {
        // renders the world
        string result = "";
        // render the world while overwriting the previous frame char by char,
        // starting at the top left of the console.
        result += $"x{String.Concat(Enumerable.Repeat("-", World.Width))}x\n";
        for (int y = 0; y < World.Height; y++) {
            result += "|";
            for (int x = 0; x < World.Width; x++) {
                result += World.GetLocation(x, y).Render_Sprite_Char();
            }
            result += "|\n";
        }
        result += $"x{String.Concat(Enumerable.Repeat("-", World.Width))}x\n";


        return result;
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
        
        Console.SetCursorPosition(Console.WindowLeft, Console.WindowTop);
        Console.Write(RenderFrame());
        Console.WriteLine($"You lived with {totalAmountOfEntitiesLived} entities                                            ");
        // for (int i = 0; i < totalAmountOfCrittersLived + 1; i++) {
        //     Console.WriteLine();
        // }
        Console.WriteLine($"You lived with {totalAmountOfPlantsLived} plants                                                ");
        Console.WriteLine($"You lived with {totalAmountOfRocksLived} rocks                                                  ");
        Console.WriteLine($"You lived with {totalAmountOfCrittersLived} critters");
        Console.WriteLine($"The oldest critter was {oldestCritter.Name} and it lived for {Math.Round(oldestCritter.Age, 2)} seconds. His stats were:");
        Console.WriteLine($"- Speed: {oldestCritter.Speed}                                                                  ");
        Console.WriteLine($"- EnergyCostMultiplier: {oldestCritter.EnergyCostMultiplier}                                       ");
        Console.WriteLine($"- Died by: {oldestCritter.DeathBy}                                                             ");

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }
}