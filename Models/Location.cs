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

    public void ReceiveEntity(Entity entity) {
        Host = entity;
    }
}