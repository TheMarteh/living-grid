if (args.Length > 0)
{
    foreach (var arg in args)
    {
        Console.WriteLine($"Argument={arg}");
    }
}
else
{
    Console.WriteLine("No arguments");
}
Thread.Sleep(1000);
Console.Clear();

// create a new renderer
Renderer renderer = new Renderer(50, 14, 60);
Thread.Sleep(1000);
renderer.Render();
