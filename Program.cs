class Tile
{
    bool revealed;
    bool bomb;
    bool flagged;
    int value;


    public Tile(int value, bool bomb = false, bool revealed = false)
    {
        this.value = value;
        this.bomb = bomb;
        this.revealed = revealed;
    }

    public bool Reveal()
    {
        this.revealed = true;
        return this.bomb;
    }

    public bool flag()
    {
        this.flagged = true;
        return true;
    }

}

class GameBoard
{
    int size;
    int numberOfMines;
    int numberOfFlags;
    int totalTiles;
    int revealedTiles;
    Tile[,] tiles;

    public GameBoard(int size, int mines)
    {
        this.size = size;
        this.numberOfMines = mines;
        this.totalTiles = this.size * this.size;

        tiles = new Tile[this.size, this.size];

        
    }


}