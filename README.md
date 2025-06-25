# Minesweeper

  

A simple implementation of minesweeper.

  

To use simply run the following in the minesweeper directory:

``` 
dotnet run
```
This will show the settings window, which allows you to choose the difficulty level of the game.  

There are four options, each affecting the board size and the number of mines that are placed:

| Difficulty | Board Size | Mine Count |
|---|---|---|
|Easy|9 x 9| 10|
|Medium|16 x 16|40|
|Hard|30 x 16|99|
|Extreme|55 x 30|400|

Once the difficulty level is selected, press the start button to generate the gameboard.


### Controls
- Left click to reveal a tile, right click to flag a tile.
- The first click is guaranteed to be clear

## Features

### Click to reveal
- If a numbered tile has the same number of flags around it as its value, then clicking on that tile will reveal all hidden tiles around it.
- If there is a mine among those tiles, it will be triggered

### Menu Bar
- Remaining mine count shows number of flags remaining to cover all mines. This counts regardless of if the flagged tiles are bombs or not
- Includes back button to go back to the settings page. This abandons the current minesweeper game
- Win state given by a face asset, frowning if you lose and having sunglasses if you win

|Neutral| Win |Lose |
|:---:|:---:|:---:|
|<img src="assets/smile.png" alt="Neutral Smile" width="35" />|<img src="assets/won.png" alt="Winning Smile" width="35" />|<img src="assets/lost.png" alt="Losing Smile" width="35" />|