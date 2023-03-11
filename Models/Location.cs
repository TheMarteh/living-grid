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

    public double Update(double dt) {
        if (Host is not IUpdatable) {
            return dt;
        } else {
            return ((IUpdatable)Host).Update(dt);
        }
    }

    public Location ReceiveEntity(IEntity entity) {
        // presents an entity to the location
        
        // if the location is occupied, the incoming entity decides what to do
        if (Host != null) {
            Host = entity.discoverEntityOn(Host, this);
        }
        else {
            Host = entity;
        }
        Host.Location = this;
        return this;
    }
    public void RemoveEntity() {
        Host = null;
    }
}