using System; 
using Gdk;
using Gtk;
using Window = Gtk.Window;
using static Gtk.Orientation;

enum Difficulty { EASY, MEDIUM, HARD, EXTREME };
public class SettingsWindow : Window
{
    int length;
    int height;
    int bombs;
    int square;
    Difficulty difficulty;

    public SettingsWindow() : base("Minesweeper")
    {
        square = 34;
        setWidgets();
    }

    void setWidgets()
    {
        Box vbox = new Box(Vertical, 40);
        Icon = new Pixbuf("assets/bomb.png");


        Box hbox = new Box(Horizontal, 20);
        RadioButton easy = new RadioButton("Easy");
        RadioButton medium = new RadioButton(easy, "Medium");
        RadioButton hard = new RadioButton(easy, "Hard");
        RadioButton extreme = new RadioButton(easy, "Extreme");

        Box startBox = new Box(Orientation.Horizontal, 100);
        Button startButton = new Button("Start");
        startBox.Add(startButton);
        startButton.Clicked += startClick;

        easy.Clicked += easyClicked;
        medium.Clicked += mediumClicked;
        hard.Clicked += hardClicked;
        extreme.Clicked += extremeClicked;

        hbox.Add(easy);
        hbox.Add(medium);
        hbox.Add(hard);
        hbox.Add(extreme);

        vbox.Add(hbox);
        vbox.Add(startBox);
        Add(vbox);
    }

    void startClick(object? sender, EventArgs e)
    {
        Hide();
        GameWindow gameWindow = new GameWindow(length, height, bombs, square);   
        gameWindow.ShowAll();
    }

    void easyClicked(object? sender, EventArgs e)
    {
        length = height = 9;
        bombs = 10;
    }
    void mediumClicked(object? sender, EventArgs e)
    {
        length = height = 16;
        bombs = 40;
    }
    void hardClicked(object? sender, EventArgs e)
    {
        square = 27;
        length = 30;
        height = 16;
        bombs = 99;
    }
    void extremeClicked(object? sender, EventArgs e)
    {
        square = 20;
        length = 55;
        height = 30;
        bombs = 400;
    }
    protected override bool OnDeleteEvent(Event ev)
    {
        Application.Quit();
        return true;
    }

    public static void Run()
    {
        Application.Init();
        SettingsWindow window = new SettingsWindow();
        window.ShowAll();
        Application.Run();
    }
}