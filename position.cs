public class Pos
{
    public int x;
    public int y;
    public Pos(int x, int y) { this.x = x; this.y = y; }

    public Pos Add(Pos newPos)
    {
        Pos result = new Pos(x + newPos.x, y + newPos.y);

        return result;
    }
    public override string ToString() => $"({x}, {y})";

     public (int, int) coord()
    {
        return (x, y);
    }
}