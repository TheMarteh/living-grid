public abstract class Entity : IEntity, ITinySprite
{   public string Name { get; set;}
    public Location? Host { get; set; }
    public bool IsAlive { get; set;}
    public Entity(string name)
    {
        Name = name;
        onSpawn();
    }
    public virtual char Render_Sprite_Char()
    {
        return ' ';
    }
    public abstract void onSpawn();
    public abstract IEntity discoverEntityOn(IEntity e, Location loc);
    public abstract void PerformAction(double dt);
}

public interface IEntity
{
    public Location? Host { get; set; }
    public bool IsAlive { get; set;}

    public void onSpawn();
    public IEntity discoverEntityOn(IEntity e, Location loc);
    public void PerformAction(double dt);
}