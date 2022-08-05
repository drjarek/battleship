# BATTLESHIP GAME

## How to run game via script
Execute run_game.ps1 script from root folder

## How to run game manually
1) build project
execute 'dotnet build --configuration Release'

2) publish project
execute 'dotnet publish --configuration Release'

3) execute './src/ConsoleApi/bin/Release/net5.0/ConsoleApi'

## How to play game
The program will create a 10x10 grid, and place several ships on the grid at random with the following sizes:
1x Battleship (5 squares)
2x Destroyers (4 squares)

The player enters coordinates of the form “A5”, where “A” is the column and “5” is the row, to specify a square to target