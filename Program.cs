using System;

public class Tile
{
    public bool revealed { get; set; }
    public bool bomb { get; private set; }
    public bool flagged { get; set; }
    public bool firstBomb { get; set; } = false;
    public int value;

    public Tile(int value = 0, bool bomb = false, bool revealed = false)
    {
        this.value = value;
        this.bomb = bomb;
        this.revealed = revealed;
    }

    public void setValue(int value)
    {
        this.value = value;
        if (this.value == -1)
        {
            bomb = true;
        }
        return;
    }

    public bool Reveal()
    {
        revealed = true;
        return bomb;
    }

    public void toggleFlag()
    {
        if (!revealed)
        {
            flagged = !flagged;
        }
    }

}

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

public class GameBoard
{
    public int winState { get; set; } = 0;
    public int size { get; }
    int mineCount;
    public int flagCount;
    int totalTiles;
    int revealedTiles;
    public Tile[,] tiles { get; private set; }
    Pos[] bombs;
    Pos? initialClick;
    Pos[] adjTiles = [new Pos(-1, -1), new Pos(0, -1), new Pos(1, -1),
                      new Pos(-1, 0),                  new Pos(1, 0),
                      new Pos(-1, 1),  new Pos(0, 1),  new Pos(1, 1)];

    public GameBoard(int size, int mines)
    {
        this.size = size;
        mineCount = mines;
        totalTiles = this.size * this.size;

        tiles = new Tile[this.size, this.size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                tiles[i, j] = new Tile(0);
            }
        }
        bombs = new Pos[mineCount];

    }
    public Tile tileAtPos(Pos pos) => tiles[pos.x, pos.y];
    bool inRange(int x, int y) => (x >= 0 && x < size) && (y >= 0 && y < size);

    public void setBombs()
    {
        Random rnd = new Random();
        int placed = 0;

        while (placed < mineCount)
        {
            int x = rnd.Next(0, size);
            int y = rnd.Next(0, size);

            if (Math.Abs(x - initialClick!.x) <= 1 && Math.Abs(y - initialClick.y) <= 1)
                continue;

            if (!tiles[x, y].bomb && !((x, y) == initialClick.coord()))
            {
                tiles[x, y].setValue(-1);
                bombs[placed] = new Pos(x, y);
                placed++;
            }
        }
    }

    public void setValues()
    {
        foreach (Pos bomb in bombs)
        {
            foreach (Pos tile in adjTiles)
            {
                Pos nextTile = bomb.Add(tile);
                int x = nextTile.x;
                int y = nextTile.y;
                if (inRange(x, y) && (!tiles[x, y].bomb))
                {
                    tiles[x, y].value++;
                }
            }
        }
    }

    public void printBoard(int a, int b)
    {
        initialClick = new Pos(a, b);
        setBombs();
        setValues();
        initialReveal(initialClick);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (tiles[x, y].revealed)
                {
                    int value = tiles[x, y].value;

                    if (value == -1)
                    {
                        Console.Write("X ");
                    }
                    else
                    {
                        Console.Write(value + " ");
                    }
                }
                else
                {
                    Console.Write("[]");
                }


            }
            Console.WriteLine();
        }
    }

    int adjFlags(Pos pos)
    {
        int count = 0;
        foreach (Pos adj in adjTiles)
        {
            Pos nextTile = pos.Add(adj);
            int x = nextTile.x;
            int y = nextTile.y;
            if (inRange(x, y) && tiles[x, y].flagged)
            {
                count++;
            }
        }
        return count;
    }

    public void revealAdj(Pos start, out Pos? bomb)
    {
        Pos? tempBomb = null;

        Tile tile = tileAtPos(start);
        if (adjFlags(start) == tile.value)
        {
            foreach (Pos adj in adjTiles)
            {
                Pos nextTile = start.Add(adj);
                int x = nextTile.x;
                int y = nextTile.y;
                if (inRange(x, y) && (!tiles[x, y].flagged))
                {
                    if (tiles[x, y].bomb)
                        tempBomb = new Pos(x, y);
                    revealEmpty(nextTile, out bomb);
                }
            }
        }
        bomb = tempBomb;
    }


    public void revealEmpty(Pos start, out Pos? bomb)
    {

        if (tiles[start.x, start.y].revealed)
        {
            bomb = null;
            return;
        }
        Pos? tempBomb = null;
        tiles[start.x, start.y].Reveal();

        if (tiles[start.x, start.y].value == 0)
        {
            foreach (Pos adj in adjTiles)
            {
                Pos nextTile = start.Add(adj);
                int x = nextTile.x;
                int y = nextTile.y;
                if (inRange(x, y) && (!tiles[x, y].flagged))
                {
                    revealEmpty(nextTile, out bomb);
                    if (tiles[x, y].bomb)
                        tempBomb = new Pos(x, y);
                }
            }
        }
        bomb = tempBomb;
    }

    public void initialReveal(Pos start)
    {
        initialClick = start;
        setBombs();
        setValues();
        revealEmpty(start, out _);
    }

    public void testGenerate()
    {
        Random rnd = new Random();

        int x = rnd.Next(0, size);
        int y = rnd.Next(0, size);

        printBoard(x, y);
    }

    public void reveal(int x, int y)
    {
        Pos pos = new Pos(x, y);
        revealEmpty(pos, out _);
    }

}

class Program
{
    static void Main(string[] args)
    {
        GameWindow.run(10, 10); // first number represents the length of the square used in tiles, and the second number represents number of bombs
    }
}