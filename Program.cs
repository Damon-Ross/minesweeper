using System;

public class GameBoard
{
    public int winState { get; set; } = 0;
    public int length { get; }
    public int height { get; }
    public int mineCount { get; }
    public int flagCount;
    int totalTiles;
    int revealedTiles = 0;
    public Tile[,] tiles { get; private set; }
    Pos[] bombs;
    Pos? initialClick;
    Pos[] adjTiles = [new Pos(-1, -1), new Pos(0, -1), new Pos(1, -1),
                      new Pos(-1, 0),                  new Pos(1, 0),
                      new Pos(-1, 1),  new Pos(0, 1),  new Pos(1, 1)];

    public GameBoard(int mineCount, int length, int height)
    {
        this.length = length;
        this.height = height;
        this.mineCount = mineCount;
        totalTiles = this.length * this.height;

        tiles = new Tile[height, length];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                tiles[y, x] = new Tile(0);
            }
        }

        bombs = new Pos[mineCount];
    }

    public Tile tileAtPos(Pos pos) => tiles[pos.y, pos.x];

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

            if (!tiles[y, x].bomb && !((x, y) == initialClick.coord()))
            {
                tiles[y, x].setValue(-1);
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
                if (inRange(x, y) && (!tiles[y, x].bomb))
                {
                    tiles[y, x].value++;
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

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                if (tiles[y, x].revealed)
                {
                    int value = tiles[y, x].value;
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
            if (inRange(x, y) && tiles[y, x].flagged)
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
                if (inRange(x, y) && (!tiles[y, x].flagged))
                {
                    if (tiles[y, x].bomb)
                        tempBomb = new Pos(x, y);
                    revealEmpty(nextTile, out bomb);
                }
            }
        }
        bomb = tempBomb;
    }

    public void revealEmpty(Pos start, out Pos? bomb)
    {
        if (tiles[start.y, start.x].revealed)
        {
            bomb = null;
            return;
        }

        Pos? tempBomb = null;
        tiles[start.y, start.x].Reveal();
        revealedTiles++;

        if (tiles[start.y, start.x].value == 0)
        {
            foreach (Pos adj in adjTiles)
            {
                Pos nextTile = start.Add(adj);
                int x = nextTile.x;
                int y = nextTile.y;
                if (inRange(x, y) && (!tiles[y, x].flagged))
                {
                    revealEmpty(nextTile, out bomb);
                    if (tiles[y, x].bomb)
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

    public void checkWin()
    {
        if (flagCount + revealedTiles == totalTiles)
        {
            winState = 1;
        }
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

        // GameBoard game = new GameBoard(10, 20, 5);
        // game.testGenerate();
        // GameWindow.run(20, 5, 15, 35); // first number represents the length of the square used in tiles, and the second number represents number of bombs
        SettingsWindow.Run();
    }
}