public class Rock : Entity
{
    private bool isLarge;
    public Rock(string name) : base(name)
    {
    }

    public override void onSpawn()
    {
        // a rock has a 1 in 4 chance to spawn as large rock
        var random = new Random();
        isLarge = random.Next(0,3) == 1;
        return;
    }

    public override char Render_Sprite_Char()
    {
        if (isLarge) return '0';
        return 'o';
    }

    public override IEntity discoverEntityOn(IEntity e, Location loc) {
        this.Host = loc;
        return this;
    }

    public override void PerformAction(double dt) {
        // do nothing
    }
}