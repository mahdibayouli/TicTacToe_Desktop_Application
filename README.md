# TicTacToe Desktop Application 
## Table of contents
* [General info](#general-info)
* [Overview](#Overview)
* [Technologies](#technologies)
* [Run the application](#Run-the-application)

## General info
This project is a TicTacToe desktop application. It has three game modes: easy, hard and multiplayer.
In easy mode, the computer randomly chooses an empty field, whereas in hard mode it uses an AI implemented with the [minimax algorithm](https://en.wikipedia.org/wiki/Minimax). 
In both these single player modes, the user can either choose to play with X or with O.

## Overview

This is a brief description of the different parts of the game, feel free to watch the short animated GIF down below for a quick game walkthrough.

When you run the application, you should first of all choose the game mode in order to start a new game. 

The game can either be single player (Easy / Hard modes) or multiplayer. After choosing a single player mode, you should first chose X or O to start a game. 

The score is updated after each turn. You can always reset the score to 0 by clicking the button at the bottom right.

You can also switch between game modes from the list at the top.

![TicTacToe_Gif](https://user-images.githubusercontent.com/76594745/164062103-ccd90fc0-b8d9-4feb-876d-6c343baca357.gif)


## Technologies
This project is created with:
* C#
* WPF Framework (.NET 5.0)

## Run the application
To run this application, [.NET must be installed on your computer](https://docs.microsoft.com/en-us/dotnet/framework/install/dotnet-35-windows).

Once you've installed .NET, you can now run the executable file and try the game yourself!

To do that, open the Terminal in the current repository and type the folowing command:
```
./Application/WPFApp.exe
```



