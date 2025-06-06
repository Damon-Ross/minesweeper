using Gdk;
using Gtk;
using System;
using Window = Gtk.Window;

class Assets {
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

public class GameWindow : Window
{
    GameBoard board;
    Grid grid;
    const int Square = 34;
    Assets assets;

    public GameWindow(int size, int mines) : base("minesweeper")
    {
        board = new GameBoard(size, mines);
        Resize(Square * board.getSize(), Square * board.getSize());
        assets = new Assets(Square);
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
                grid.Attach(new Image(assets.tile), x, y, 1, 1);
            }
        }
        return grid;
    }

    protected override bool OnDeleteEvent(Event ev)
    {
        Application.Quit();
        return true;
    }
    
    public static void run(int size, int mines) {
        Application.Init();
        GameWindow w = new GameWindow(size, mines);
        w.ShowAll();
        Application.Run();
    }

}