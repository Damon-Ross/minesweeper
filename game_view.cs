using System;
using Cairo;
using Gdk;
using Gtk;
using Window = Gtk.Window;
using static Gtk.Orientation;
using System.Numerics;

class Assets
{
    public Pixbuf[] numbers = new Pixbuf[9];
    public Pixbuf bomb, flag, tile, firstBomb, lost, smile, won, back;

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
        lost = new Pixbuf("assets/lost.png", size, size);
        smile = new Pixbuf("assets/smile.png", size, size);
        won = new Pixbuf("assets/won.png", size, size);
        back = new Pixbuf("assets/back.png", size, size);
    }
}

public class GameWindow : Window
{
    int Square;
    int length;
    int height;
    GameBoard board;
    Grid grid;
    Box menuBox;
    EventBox[,] tileBoxes;
    Image[,] tileImages;
    Assets assets;
    Label? bombCountLabel;
    Image? iconImage;
    bool isFirstClick = true;

    public GameWindow(int length, int height, int mineCount, int square) : base("Minesweeper")
    {
        
        Square = square;
        Icon = new Pixbuf("assets/bomb.png");
        this.length = length;
        this.height = height;
        board = new GameBoard(mineCount, length, height);
        assets = new Assets(Square);

        // Resize(Square * length, Square * (height + 1));

        tileBoxes = new EventBox[height, length];
        tileImages = new Image[height, length];
        grid = new Grid();
        menuBox = new Box(Horizontal, 70);
        createMenu();
        createGrid();
        Box vbox = new Box(Vertical, 10);
        vbox.Add(menuBox);
        vbox.Add(grid);
        Add(vbox);
    }
    void createMenu()
    {
        bombCountLabel = new Label("");
        iconImage = new Image();
        Image back = new Image(assets.back);

        EventBox backButton = new EventBox();
        backButton.Add(back);
        backButton.ButtonPressEvent += (o, args) =>
        {
            Hide();
            SettingsWindow.Run();
        };

        menuBox.Add(bombCountLabel);
        menuBox.Add(iconImage);
        menuBox.Add(backButton);
        updateMenu();
    }

    void updateMenu()
    {
        bombCountLabel!.Text = $"{board.mineCount - board.flagCount}";
        if (board.winState == -1)
        {
            iconImage!.Pixbuf = assets.lost;
        }
        else if (board.winState == 1)
        {
            iconImage!.Pixbuf = assets.won;
        }
        else
        {
            iconImage!.Pixbuf = assets.smile;
        }
    
    }

    void createGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Image img = new Image(assets.tile);
                EventBox box = new EventBox();
                box.Add(img);

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
                    board.checkWin();
                    updateGrid();
                    updateMenu();
                };

                tileBoxes[y, x] = box;
                tileImages[y, x] = img;

                grid.Attach(box, x, y, 1, 1);
            }
        }
    }

    void leftClick(int x, int y)
    {
        if (board.tiles[y, x].revealed)
        {
            board.revealAdj(new Pos(x, y), out Pos? bomb);
            if (bomb != null)
            {
                board.winState = -1;
                board.tiles[bomb.y, bomb.x].firstBomb = true;
            }
        }
        if (!board.tiles[y, x].flagged)
        {
            board.reveal(x, y);
            if (board.tiles[y, x].bomb)
            {
                board.winState = -1;
                board.tiles[y, x].firstBomb = true;
            }
        }
    }

    void rightClick(int x, int y)
    {
        if (!board.tiles[y, x].revealed)
        {
            board.tiles[y, x].toggleFlag();
            board.flagCount += board.tiles[y, x].flagged ? 1 : -1;
        }
    }

    void updateGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < length; x++)
            {
                Tile tile = board.tiles[y, x];
                Image img = tileImages[y, x];
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

    public static void run(int length, int height, int mines, int square)
    {
        Application.Init();
        GameWindow window = new GameWindow(length, height, mines, square);
        window.ShowAll();
        Application.Run();
    }
}