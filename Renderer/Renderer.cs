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

        double timeSinceLastUpdate = 0.01; // delen door nul is nooit goed
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
}