using System;
using System.Collections.Generic;

class Tile
{
    bool revealed;
    bool bomb;
    bool flagged;
    int value;

    public Tile(int value = 0, bool bomb = false, bool revealed = false)
    {
        this.value = value;
        this.bomb = bomb;
        this.revealed = revealed;
    }

    public bool isBomb() => bomb;
    public int getValue() => value;
    public void increment()
    {
        value++;
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

    public bool isRevealed() => revealed;

    public void toggleFlag()
    {
        flagged = !flagged;
    }

}

class Pos
{
    public int x, y;

    public Pos(int x, int y) { this.x = x; this.y = y; }

    public override string ToString() => $"({x}, {y})";

    public Pos Add(Pos newPos)
    {
        Pos result = new Pos(x + newPos.x, y + newPos.y);

        return result;
    }

    public (int, int) coord()
    {
        return (x, y);
    }
}


class GameBoard
{
    int size;
    int mineCount;
    int flagCount;
    int totalTiles;
    int revealedTiles;
    Tile[,] tiles;
    Pos[] bombs;
    Pos initialClick;
    Pos[] adjTiles = [new Pos(-1, -1), new Pos(0, -1), new Pos(1, -1),
                        new Pos(-1, 0),                  new Pos(1, 0),
                        new Pos(-1, 1), new Pos(0, 1), new Pos(1, 1)];

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

    public void setBombs()
    {
        Random rnd = new Random();
        int placed = 0;


        while (placed < mineCount)
        {
            int x = rnd.Next(0, size);
            int y = rnd.Next(0, size);

            if (!tiles[x, y].isBomb() && !((x, y) == initialClick.coord()))
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
                if ((x >= 0 && x < size) && (y >= 0 && y < size) && (!tiles[x, y].isBomb()))
                {
                    tiles[x, y].increment();
                }
            }
        }
    }

    public void printBoard(int a, int b)
    {
        initialClick =new Pos(a, b);
        setBombs();
        setValues();
        initialReveal(initialClick);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (tiles[x, y].isRevealed())
                {
                    int value = tiles[x, y].getValue();

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

    public void revealEmpty(Pos start)
    {
        if (tiles[start.x, start.y].isRevealed())
        return;

        tiles[start.x, start.y].Reveal();

        if (tiles[start.x, start.y].getValue() == 0)
        {
            foreach (Pos adj in adjTiles)
            {
                Pos nextTile = start.Add(adj);
                int x = nextTile.x;
                int y = nextTile.y;
                if ((x >= 0 && x < size) && (y >= 0 && y < size))
                {
                    revealEmpty(nextTile);
                }
            }
        }
    }


    public void initialReveal(Pos start)
    {
        initialClick = start;
        setBombs();
        setValues();
        revealEmpty(start);

    }

}



    class Program
    {
        static void Main(string[] args)
        {
            GameBoard game = new GameBoard(25, 50);
            
            game.printBoard(5, 5);
        }
    }