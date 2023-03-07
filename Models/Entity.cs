public abstract class Entity : IEntity, ITinySprite
{   public string Name { get; set;}
    public Entity(string name)
    {
        Name = name;
    }
    public virtual char Render()
    {
        return ' ';
    }

}

public interface IEntity
{
    public string Name { get; set;}
}