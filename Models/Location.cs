public class Location : ITinySprite {
    public Entity? Host { get; private set; }
    public Location() {
        Host = null;
    }
    public char Render_Sprite_Char() {
        if (Host == null) {
            return ' ';
        } else {
            return Host.Render_Sprite_Char();
        }
    }

    public Location ReceiveEntity(IEntity entity) {
        // presents an entity to the location
        
        // if the location is empty, the entity is accepted
        if (Host == null) {
            Host = entity as Entity;
        }
        // if the location is occupied, the entity is rejected
        return this;
    }
    public void RemoveEntity() {
        Host = null;
    }
}