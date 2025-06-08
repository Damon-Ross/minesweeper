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
                        if (!board.tiles[xCopy, yCopy].isFlagged())
                            board.reveal(xCopy, yCopy);
                        if (board.tiles[xCopy, yCopy].bomb)
                        {
                            board.winState = -1;
                            board.tiles[xCopy, yCopy].firstBomb = true;
                        }
                    }
                    else if (args.Event.Button == 3)
                    {
                        board.tiles[xCopy, yCopy].toggleFlag();
                        board.flagCount++;
                    }

                    updateGrid();
                };
                

                tileBoxes[x, y] = box;
                tileImages[x, y] = img;

                grid.Attach(box, x, y, 1, 1);
            }
        }
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
                if (tile.isRevealed())
                {
                    int val = tile.value;
                    img.Pixbuf = (val == -1) ? assets.bomb : assets.numbers[val];
                    if (tile.firstBomb)
                    {
                        img.Pixbuf = assets.firstBomb;
                    }
                }
                else if (tile.isFlagged())
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
