using System;
using System.ComponentModel;
using Cairo;
using Gdk;
using Gtk;
using Window = Gtk.Window;
using static Gtk.Orientation;

enum Difficulty { EASY, MEDIUM, HARD, EXTREME };
public class SettingsWindow : Window
{
    int size;
    int bombs;
    Difficulty difficulty;

    public SettingsWindow() : base("Minesweeper")
    {
        setWidgets();
    }

    void setWidgets()
    {
        Box vbox = new Box(Vertical, 5);

        Box hbox = new Box(Orientation.Horizontal, 5);
        RadioButton easy = new RadioButton("Easy");
        RadioButton medium = new RadioButton(easy, "Medium");
        RadioButton hard = new RadioButton(easy, "Hard");
        RadioButton extreme = new RadioButton(easy, "Extreme");

        Box startBox = new Box(Orientation.Horizontal, 10);
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
        this.Hide();
        GameWindow gameWindow = new GameWindow(size, bombs);
        gameWindow.ShowAll();
    }

    void easyClicked(object? sender, EventArgs e)
    {
        size = 9;
        bombs = 10;
    }
    void mediumClicked(object? sender, EventArgs e)
    {
        size = 16;
        bombs = 40;
    }
    void hardClicked(object? sender, EventArgs e)
    {
        size = 25;
        bombs = 99;
    }
    void extremeClicked(object? sender, EventArgs e)
    {
        size = 45;
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