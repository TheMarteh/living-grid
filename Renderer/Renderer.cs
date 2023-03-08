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
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        
        // set the console properties
        
        // Console.SetWindowSize(Width + 20, Height + 4);
        // Console.SetBufferSize(Width + 20, Height + 4);
        Console.CursorVisible = false;
        int loopStartTime;
        double timeSinceLastUpdate = 0.01; // delen door nul is nooit goed
        int loopEndTime = stopwatch.Elapsed.Milliseconds;
        Thread.Sleep(10);

        while (stopwatch.Elapsed.TotalSeconds < 30) {
            // update the world while passing the time since last update;
            var tickTimer = new Stopwatch();
            tickTimer.Start();

            World.Update(timeSinceLastUpdate);

            // render the world
            double dt = RenderFrame();

            // calculate how long we need to wait to not go over
            // the target fps
            double targetFrameTime = 1000.0 / TargetFps;
            double timeToWait = dt < targetFrameTime 
                ? targetFrameTime - dt
                : 0;

            double frameTime = timeToWait + dt;
            Console.WriteLine($" fps: {(1000/frameTime)}/{TargetFps}");
            Console.WriteLine($"Rendered frame in {dt} ms");
            
            // update the world
            World.Update(frameTime);

            Thread.Sleep((int)timeToWait);
            timeSinceLastUpdate = (tickTimer.ElapsedMilliseconds / 1000);
        }
    }

    public double RenderFrame() {
        // renders the world
        // return the time it took to render the frame
        double dt;
        Stopwatch stopwatch = new Stopwatch();

        // start the stopwatch
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
}