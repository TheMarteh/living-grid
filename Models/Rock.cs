public class Rock : Entity, ITinySprite
{
    public Rock(string name) : base(name)
    {
    }

    public override char Render()
    {
        return 'O';
    }
}