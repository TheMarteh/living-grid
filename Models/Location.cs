public class Location : ITinySprite {
    public int X { get; private set; }
    public int Y { get; private set; }
    public int CellSize { get; private set; }
    public IEntity? Visitor { get; private set; }
    public World World {get; set;}
    public Location(World world, int x, int y) {
        Visitor = null;
        World = world;
        X = x;
        Y = y;
        CellSize = world.CellSize;

    }
    public char Render_Sprite_Char() {
        if (Visitor is not ITinySprite) {
            return ' ';
        } else {
            return ((ITinySprite)Visitor).Render_Sprite_Char();
        }
    }

    public double Update(double dt) {
        if (Visitor is not IUpdatable) {
            return dt;
        } else {
            return ((IUpdatable)Visitor).Update(dt);
        }
    }

    public Location ReceiveEntity(IEntity entity) {
        // presents an entity to the location
        // if (entity is IMovable)
        // {
        //     ((IMovable)entity).relativeXPosition = 0;
        //     ((IMovable)entity).relativeYPosition = 0;
        // }
        
        // if the location is occupied, the incoming entity decides what to do.

        if (this.Visitor != null) {
            this.Visitor = entity.discoverEntityOn(Visitor, this);
        }
        else {
            this.Visitor = entity;
        }
        this.Visitor.Host = this;
        // ((IMovable)this.Visitor).relativeXPosition = 0;
        // ((IMovable)this.Visitor).relativeXPosition = 0;
        return this;
    }
    public void RemoveEntity() {
        Visitor = null;
    }

    public Location CheckBorderCrossing(double relX, double relY) {
        // check if the critter has crossed a border
        // if so, move it to the next location
        if (Math.Abs(relX) > CellSize / 2|| Math.Abs(relY) > CellSize / 2 ) {
            // the critter has crossed a border

            // als de visitor null is, is hij succesvol weggegeven. Anders moeten we er nog iets mee doen.
            this.Visitor =  World.MoveEntity(this, Visitor as Entity);
            if (Visitor is not null) {
                ((IMovable)Visitor).Bounce();
                return this;
            }
            return null;
        }
        return this;
    }
}