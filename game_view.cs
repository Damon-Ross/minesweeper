using System;
using Cairo;
using Gdk;
using Gtk;
using Window = Gtk.Window;

class Assets
{
    public Pixbuf[] numbers = new Pixbuf[9];
    public Pixbuf? bomb, flag, tile, firstBomb;

    public Assets(int size)
    {
        numbers[0] = new Pixbuf("assets/blank.png", size, size);
        for (int i = 1; i < 9; i++)
        {
            numbers[i] = new Pixbuf($"assets/{i}.png", size, size);
        }

        bomb = new Pixbuf("assets/bomb.png", size, size);
        flag = new Pixbuf("assets/flag.png", size, size);
        tile = new Pixbuf("assets/tile.png", size, size);
        firstBomb = new Pixbuf("assets/firstBomb.png", size, size);
    }
}

public class GameWindow : Window
{
    const int Square = 34;
    int size;
    GameBoard board;
    Grid grid;
    EventBox[,] tileBoxes;
    Image[,] tileImages;
    Assets assets;
    bool isFirstClick = true;

    public GameWindow(int size, int mineCount) : base("Minesweeper")
    {
        this.size = size;
        board = new GameBoard(size, mineCount);
        assets = new Assets(Square);

        Resize(size * Square, size * Square);

        tileBoxes = new EventBox[size, size];
        tileImages = new Image[size, size];
        grid = new Grid();

        createGrid();
        Add(grid);
    }

    void createGrid()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Image img = new Image(assets.tile);
                EventBox box = [img];

                int xCopy = x;
                int yCopy = y;
                
                box.ButtonPressEvent += (o, args) =>
                {
                    if (board.winState == -1)
                    {
                        return;
                    }
                    if (isFirstClick)
                    {
                        board.initialReveal(new Pos(xCopy, yCopy));
                        isFirstClick = false;
                    }

                    if (args.Event.Button == 1)
                    {
                        leftClick(xCopy, yCopy);
                    }
                    else if (args.Event.Button == 3)
                    {
                        rightClick(xCopy, yCopy);
                    }
                    updateGrid();
                };
                

                tileBoxes[x, y] = box;
                tileImages[x, y] = img;

                grid.Attach(box, x, y, 1, 1);
            }
        }
    }
    void leftClick(int x, int y)
    {
        if (board.tiles[x, y].revealed)
        {
            board.revealAdj(new Pos(x, y), out Pos? bomb);
            if (!(bomb == null))
            {
                board.winState = -1;
                board.tiles[bomb.x, bomb.y].firstBomb = true;
            }
        }
        if (!board.tiles[x, y].flagged)
        {
            board.reveal(x, y);
            if (board.tiles[x, y].bomb)
            {
                board.winState = -1;
                board.tiles[x, y].firstBomb = true;
            }
        }
    }

    void rightClick(int x, int y)
    {
        board.tiles[x, y].toggleFlag();
        board.flagCount++;
    }

    void updateGrid()
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                Tile tile = board.tiles[x, y];
                Image img = tileImages[x, y];
                if (board.winState == -1)
                {
                    if (tile.bomb)
                    {
                        tile.Reveal();
                    }
                }
                if (tile.revealed)
                {
                    int val = tile.value;
                    img.Pixbuf = (val == -1) ? assets.bomb : assets.numbers[val];
                    if (tile.firstBomb)
                    {
                        img.Pixbuf = assets.firstBomb;
                    }
                }
                else if (tile.flagged)
                {
                    img.Pixbuf = assets.flag;
                }
                else
                {
                    img.Pixbuf = assets.tile;
                }
            }
        }
    }

    protected override bool OnDeleteEvent(Event ev)
    {
        Application.Quit();
        return true;
    }

    public static void run(int size, int mines)
    {
        Application.Init();
        GameWindow window = new GameWindow(size, mines);
        window.ShowAll();
        Application.Run();
    }
}
