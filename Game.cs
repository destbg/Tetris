using System;
using System.Timers;
using static System.Console;

namespace Tetris {
    class Game {
        private Board board;
        private Timer timer;
        private Timer rotate;
        private bool allowed;
        private bool canRotateAndPlace;

        public Game() {
            StartScreen();
            while (StartUp.GameState) {
                var input = ReadKey(true).Key;
                allowed = false;
                switch (input) {
                    case ConsoleKey.W: {
                        if (canRotateAndPlace) {
                            board.RotateBlock();
                            SetCursorPosition(0, 0);
                            WriteLine(board);
                            canRotateAndPlace = false;
                        }
                        break;
                    }
                    case ConsoleKey.A: {
                        board.MoveBlock(2);
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        break;
                    }
                    case ConsoleKey.S: {
                        board.MoveBlock(0);
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        break;
                    }
                    case ConsoleKey.D: {
                        board.MoveBlock(1);
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        break;
                    }
                    case ConsoleKey.Spacebar: {
                        if (canRotateAndPlace) {
                            board.InstantlyPlaceBlock();
                            SetCursorPosition(0, 0);
                            WriteLine(board);
                            canRotateAndPlace = false;
                        }
                        break;
                    }
                    case ConsoleKey.Escape: {
                        Menu();
                        WindowWidth = 33;
                        WindowHeight = 25;
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        break;
                    }
                }
                allowed = true;
            }
            allowed = false;
            WindowHeight = 30;
            WindowWidth = 120;
            WriteLine("\nGame Ended\n" +
                "Your score is: " + board.GetScore() + '\n' +
                "Press 'R' to restart the game.");
            while (ReadKey(true).Key != ConsoleKey.R) { }
        }

        private void StartScreen() {
            Clear();
            WindowHeight = 30;
            WindowWidth = 120;
            board = new Board();
            WriteLine("Welcome to C# console tetris by destbg.\n" +
                "While in-game you can access the menu by pressing the 'escape' button.\n" +
                "To start the game type the difficulty you want to begin with.\n" +
                "Types of difficulty: easy, normal, average, hard.\n");
            int difficulty = 1000;
            string input;
            while (true) {
                Write("difficulty: ");
                input = ReadLine().Trim().ToLower();
                if (input == "easy") break;
                else if (input == "normal") {
                    difficulty = 750;
                    break;
                }
                else if (input == "average") {
                    difficulty = 500;
                    break;
                }
                else if (input == "hard") {
                    difficulty = 300;
                    break;
                }
                else WriteLine("Wrong difficulty input, try again.\n");
            }
            Clear();
            allowed = true;
            canRotateAndPlace = true;
            WindowWidth = 33;
            WindowHeight = 23;
            SetCursorPosition(0, 0);
            WriteLine(board);
            timer = new Timer() {
                Interval = difficulty,
                AutoReset = true,
                Enabled = true
            };
            rotate = new Timer() {
                Interval = 200,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += TimerTick;
            rotate.Elapsed += Rotate_Elapsed;
        }

        private void Rotate_Elapsed(object sender, ElapsedEventArgs e) =>
            canRotateAndPlace = true;

        private void TimerTick(object sender, ElapsedEventArgs e) {
            if (allowed) {
                board.MoveBlock(0);
                SetCursorPosition(0, 0);
                WriteLine(board);
            }
        }

        private void Menu() {
            WindowHeight = 30;
            WindowWidth = 120;
            while (true) {
                Clear();
                WriteLine("    Paused\n" +
                    "Press '1' to resume\n" +
                    "Press '2' for 'how to play'\n" +
                    "Press '3' to quit");
                var input = ReadKey(true).Key;
                if (input == ConsoleKey.D1) break;
                else if (input == ConsoleKey.D2) {
                    Clear();
                    WriteLine("1: Move the blocks left and right by presssing 'a' and 'd'\n\n" +
                        "2: Rotate the block by pressing 'w'\n\n" +
                        "3: Move the blocks faster by pressing 's'\n\n" +
                        "4: Move the blocks instantly by pressing 'space'");
                    ReadKey();
                }
                else if (input == ConsoleKey.D3) {
                    StartUp.GameState = false;
                    break;
                }
            }
        }
    }
}