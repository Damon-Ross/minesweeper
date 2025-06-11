using System;
using Cairo;
using Gdk;
using Gtk;
using Window = Gtk.Window;

enum Difficulty { EASY, MEDIUM, HARD, EXTREME };
public class SettingsWindow : Window
{
    int size;
    Difficulty difficulty;
    
    public SettingsWindow() : base("Minesweeper")
    {

    }
}