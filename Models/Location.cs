public class Location : ITinySprite {
    public Entity? Host { get; private set; }
    public Location() {
        Host = null;
    }
    public char Render() {
        if (Host == null) {
            return ' ';
        } else {
            return Host.Render();
        }
    }

    public void ReceiveEntity(Entity entity) {
        Host = entity;
    }
}