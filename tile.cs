public class Tile
{
    public bool revealed { get; set; }
    public bool bomb { get; private set; }
    public bool flagged { get; set; }
    public bool firstBomb { get; set; } = false;
    public int value;

    public Tile(int value = 0, bool bomb = false, bool revealed = false)
    {
        this.value = value;
        this.bomb = bomb;
        this.revealed = revealed;
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

    public void toggleFlag()
    {
        if (!revealed)
        {
            flagged = !flagged;
        }
    }

}