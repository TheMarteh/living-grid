public class Critter : Entity, IMovable
{
    public int Speed { get; set; }

    public double relativeXPosition { get; set; }
    public double relativeYPosition { get; set; }
    public double Direction { get; set; }

    public Critter(string name, int speed) : base(name)
    {
        Speed = speed;
    }
    public override void onSpawn()
    {
        relativeXPosition = 0;
        relativeYPosition = 0;
        double randomDirection = new Random().NextDouble() * 2 * Math.PI;
        Direction = randomDirection;
    }
    public void Move(double dt)
    {
        Console.WriteLine("The critter named " + Name + " has moved!");
        relativeXPosition += (Speed * dt * Math.Cos(Direction));
        relativeYPosition += (Speed * dt * Math.Sin(Direction));
    }

    public override char Render_Sprite_Char()
    {
        return 'C';
    }
}