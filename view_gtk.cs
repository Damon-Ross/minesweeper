using Gdk;
using Gtk;
using System;
using Window = Gtk.Window;

class Assets
{
    public Pixbuf[] numbers = new Pixbuf[9];
    public Pixbuf? bomb, flag, tile;

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
    }
}

public enum tileStates{ HIDDEN, REVEALED, FLAGGED };

public class TileWidget : EventBox
{
    int x;
    int y;
    int size;
    int value;
    Pos position;
    tileStates state;
    Image image;
    Assets assets;

    public TileWidget(int x, int y, int size, int value = 0)
    {
        position = new Pos(x, y);
        state = tileStates.HIDDEN;
        this.size = size;
        assets = new Assets(this.size);
        image = new Image(assets.tile);
        Add(image);

        Events |= EventMask.ButtonPressMask;
        ButtonPressEvent += OnClick;

    }

    void OnClick(object o, ButtonPressEventArgs args)
    {
        if (args.Event.Button == 1)
        {
            reveal();
        }
        else if (args.Event.Button == 3)
        {
            flag();
        }
    }

    public bool exploded() => state == tileStates.REVEALED && value == -1;
    public Pos getPos() => position;
    public int Value
    {
        get => value;
        set => this.value = value;
    }
    
    public void reveal()
    {
        if (state == tileStates.HIDDEN)
        {
            if (value == -1)
            {
                image.Pixbuf = assets.bomb;
                state = tileStates.REVEALED;
                return;
            }
            image.Pixbuf = assets.numbers[value];
            state = tileStates.REVEALED;
        }

    }

    public void flag()
    {
         if (state == tileStates.HIDDEN)
        {
            image.Pixbuf = assets.flag;
            state = tileStates.FLAGGED;
        } else if (state == tileStates.FLAGGED)
        {
            image.Pixbuf = assets.tile;
            state = tileStates.HIDDEN;
        }
    }
}

public class GameWindow : Window
{
    bool isFirstClick = true;
    GameBoard board;
    Grid grid;
    int size;
    const int Square = 34;
    Assets assets;
    TileWidget[,] gridArray;

    public GameWindow(int size, int mines) : base("minesweeper")
    {
        this.size = size;
        board = new GameBoard(size, mines);
        Resize(Square * board.getSize(), Square * board.getSize());
        assets = new Assets(Square);
        gridArray = new TileWidget[size, size]!;
        grid = makeGrid();
        Add(grid);

    }

    Grid makeGrid()
    {
        Grid grid = new Grid();

        for (int x = 0; x < board.getSize(); x++)
        {
            for (int y = 0; y < board.getSize(); y++)
            {
                gridArray[x, y] = new TileWidget(x, y, Square);
                grid.Attach(new TileWidget(x, y, Square), x, y, 1, 1);
            }
        }
        return grid;
    }

    public void setGridValues()
    {
        for (int x = 0; x < board.getSize(); x++)
        {
            for (int y = 0; y < board.getSize(); y++)
            {
                gridArray[x, y].Value = board.tile(x, y).getValue();
            }
        }
    }

    public void updateGrid()
    {
        for (int x = 0; x < board.getSize(); x++)
        {
            for (int y = 0; y < board.getSize(); y++)
            {
                
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
        GameWindow w = new GameWindow(size, mines);
        w.ShowAll();
        Application.Run();
    }
}