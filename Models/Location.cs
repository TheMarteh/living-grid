public class Location : ITinySprite {
    public IEntity? Host { get; private set; }
    public Location() {
        Host = null;
    }
    public char Render_Sprite_Char() {
        if (Host is not ITinySprite) {
            return ' ';
        } else {
            return ((ITinySprite)Host).Render_Sprite_Char();
        }
    }

    public Location ReceiveEntity(IEntity entity) {
        // presents an entity to the location
        
        // if the location is occupied, the entity decides what to do
        if (Host != null) {
            Host = entity.discoverEntityOn(Host, this);
        }
        else {
            Host = entity;
        }
        return this;
    }
    public void RemoveEntity() {
        Host = null;
    }
}