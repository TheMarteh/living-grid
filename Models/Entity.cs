public abstract class Entity : IEntity, ITinySprite
{   public string Name { get; set;}
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
}

public interface IEntity
{
    public string Name { get; set;}
    public void onSpawn();
}