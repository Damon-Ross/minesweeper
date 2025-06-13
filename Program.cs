using System;
public class GameBoard
{
    public int winState { get; set; } = 0;
    public int length { get; }
    public int height { get; }
    int mineCount;
    public int flagCount;
    int totalTiles;
    int revealedTiles = 0;
    public Tile[,] tiles { get; private set; }
    Pos[] bombs;
    Pos? initialClick;
    Pos[] adjTiles = [new Pos(-1, -1), new Pos(0, -1), new Pos(1, -1),
                      new Pos(-1, 0),                  new Pos(1, 0),
                      new Pos(-1, 1),  new Pos(0, 1),  new Pos(1, 1)];

    public GameBoard(int mines, int length, int height = 0)
    {
        this.length = length;
        this.height = height == 0 ? length : height;

        mineCount = mines;
        totalTiles = this.length * this.height;

        tiles = new Tile[this.height, this.length];
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < height; j++)
            {
                tiles[i, j] = new Tile(0);
            }
        }
        bombs = new Pos[mineCount];

    }
    public Tile tileAtPos(Pos pos) => tiles[pos.x, pos.y];
    bool inRange(int x, int y) => (x >= 0 && x < length) && (y >= 0 && y < height);

    public void setBombs()
    {
        Random rnd = new Random();
        int placed = 0;

        while (placed < mineCount)
        {
            int x = rnd.Next(0, length);
            int y = rnd.Next(0, height);

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

        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < length; x++)
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
        revealedTiles++;

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

        int x = rnd.Next(0, length);
        int y = rnd.Next(0, height);

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
        // GameWindow.run(45, 400, 34); // first number represents the length of the square used in tiles, and the second number represents number of bombs
        SettingsWindow.Run();
    }
}