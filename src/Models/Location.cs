public class Location : ITinySprite {
    public int X { get; private set; }
    public int Y { get; private set; }
    public int CellSize { get; private set; }
    public IEntity? Occupant { get; private set; }
    public IEntity? Visitor { get; private set; }
    public List<IEntity> Graveyard {get; private set;}
    public World World {get; set;}
    public Location(World world, int x, int y) {
        Visitor = null;
        Occupant = null;
        World = world;
        X = x;
        Y = y;
        CellSize = world.CellSize;
        Graveyard = new List<IEntity>();

    }
    public char Render_Sprite_Char() {
        if (Visitor is ITinySprite) {
            return ((ITinySprite)Visitor).Render_Sprite_Char(); 
        }
        if (Occupant is ITinySprite) {
            return ((ITinySprite)Occupant).Render_Sprite_Char();
        }
        if (Graveyard.Count > 0) {
            if (Graveyard.Where(e => e is IMoving).Count() > 0) {
                return '†';
            }
            return '.';
        }
        return ' ';    
    }

    // public double Update(double dt) {
    //     if (Visitor is not IUpdatable) {
    //         return dt;
    //     } else {
    //         return ((IUpdatable)Visitor).Update(dt);
    //     }
    // }

    public Location ReceiveEntity(IEntity entity) {
        // presents an entity to the location
        // if the location is occupied, the incoming entity decides what to do.
        // returns false if the entity was denied

        if (Visitor is not null) {
            // De hele locatie zit vol..
            return this;
        }

        if (Occupant is null) {
            Occupant = entity;
            Occupant.Host = this;
            return this;
        }

        else {
            // the location is occupied
            // the incoming entity decides what to do
            Visitor = entity;
            Visitor.Host = this;
            var result = Visitor.discoverEntityOn(Occupant, this);
            if (result is null) {
                // The occupant is destroyed
                Occupant = Visitor;
                Visitor = null;
            }
            else {
                // The occupant is still alive
                Occupant = result;
            }
        }

        return this;
    }

    public void BuryEntity(IEntity e) {
        if (Occupant == e) {
            Graveyard.Add(Occupant);
            this.Occupant = null;
        }
        if (Visitor == e) {
            Graveyard.Add(Visitor);
            this.Visitor = null;
        }
    }

    public void NotifyBirth(IEntity baby) {
        this.World.Entities.Add(baby);
        this.ReceiveEntity(baby);
    }

    public Location CheckBorderCrossing(IMoving e) {
        // check if the critter has crossed a border
        // if so, move it to the next location
        if (Math.Abs(e.relativeXPosition) > CellSize / 2|| Math.Abs(e.relativeYPosition) > CellSize / 2 ) {
            // the critter has crossed a border


            // als de visitor null is, is hij succesvol weggegeven. Anders moeten we er nog iets mee doen.
            var result =  World.MoveEntity(this, e);
            if (result is not null) {
                e.Bounce();
            }

            if (Occupant == e) {
                Occupant = result;
            }

            if (Visitor == e) {
                Visitor = result;
            }
            return null;
        }
        return this;
    }
}