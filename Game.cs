using System;
using System.Timers;
using static System.Console;

namespace Tetris {
    class Game {
        private Board board;
        private Timer timer;
        private Timer rotate;
        private bool allowed;
        private bool canRotate;

        public Game() {
            StartScreen();
            while (StartUp.GameState) {
                var input = ReadKey(true).Key;
                switch (input) {
                    case ConsoleKey.W: {
                        if (canRotate) {
                            board.RotateBlock();
                            SetCursorPosition(0, 0);
                            WriteLine(board);
                            canRotate = false;
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
                        TimerTick();
                        break;
                    }
                    case ConsoleKey.D: {
                        board.MoveBlock(1);
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        break;
                    }
                    case ConsoleKey.Spacebar: {
                        board.InstantlyPlaceBlock();
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        break;
                    }
                    case ConsoleKey.Escape: {
                        allowed = false;
                        Menu();
                        WindowWidth = 33;
                        WindowHeight = 25;
                        SetCursorPosition(0, 0);
                        WriteLine(board);
                        allowed = true;
                        break;
                    }
                }
            }
            allowed = false;
            WindowHeight = 30;
            WindowWidth = 120;
            WriteLine("\nGame Ended\n" +
                "Your score is: " + board.GetScore() + '\n' +
                "Type 'end' to restart the game.");
            string read;
            while ((read = ReadLine().ToLower()) != "end") {
                SetCursorPosition(0, CursorTop - 1);
                WriteLine("".PadLeft(100));
                SetCursorPosition(0, CursorTop - 1);
            }
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
            while ((input = ReadLine().ToLower()) != "easy")
                if (input == "normal") {
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
            Clear();
            allowed = true;
            canRotate = true;
            WindowWidth = 33;
            WindowHeight = 25;
            TimerTick();
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

        private void Rotate_Elapsed(object sender, ElapsedEventArgs e) {
            canRotate = true;
        }

        private void TimerTick(object sender = null, ElapsedEventArgs e = null) {
            if (allowed) {
                board.MoveBlock(0);
                SetCursorPosition(0, 0);
                WriteLine(board);
            }
        }

        private void Menu() {
            WindowHeight = 30;
            WindowWidth = 120;
            string input = "me";
            while (input != "1") {
                Clear();
                WriteLine("    Paused\n" +
                    "Type '1' to resume\n" +
                    "Type '2' for 'how to play'\n" +
                    "Type '3' to quit\n");
                if (input == "2") {
                    Clear();
                    WriteLine("1: Move the blocks left and right by presssing 'a' and 'd'\n\n" +
                        "2: Rotate the block by pressing 'w'\n\n" +
                        "3: Move the blocks faster by pressing 's'\n\n" +
                        "4: Move the blocks instantly by pressing 'space'");
                    ReadKey();
                    input = "me";
                }
                else if (input == "3") {
                    StartUp.GameState = false;
                    break;
                }
                else input = ReadLine();
            }
        }
    }
}