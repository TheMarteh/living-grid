public interface IMovable {
    public int Speed { get; set; }
    public double Direction { get; set; }
    public double relativeXPosition { get; set; }
    public double relativeYPosition { get; set; }
    public void Move(double dt);
    public void Bounce();
}