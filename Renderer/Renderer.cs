public class Renderer
{
    public World World { get; set; }
    public int Width { get; private set; }
    public int Height { get; private set; }
    public Renderer(int width, int height)
    {
        Width = width;
        Height = height;
        World = new World(Height, Width);
        World.Populate();
    }

    public void Render() 
    {
        for (int y = 0; y < World.Height; y++) {
            for (int x = 0; x < World.Width; x++) {
                Console.Write(World.GetLocation(x, y).Render_Sprite_Char());
            }
            Console.WriteLine();
        }
    }
}