public class Rock : Entity
{
    public Rock(string name) : base(name)
    {
    }

    public override void onSpawn()
    {
        // Do nothing
        return;
    }

    public override char Render_Sprite_Char()
    {
        return 'O';
    }
}