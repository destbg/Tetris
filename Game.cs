﻿using System.Timers;
using static System.Console;
using static System.ConsoleKey;

namespace Tetris {
    class Game {
        private Board board;
        private Timer timer, move;
        private bool allowed, canMove;
        private int difficulty, level;

        private static bool gameState;
        public static bool GameState { set => gameState = value; }

        public Game() {
            #region start screen
            Clear();
            CursorVisible = true;
            GameState = true;
            WindowHeight = 30;
            WindowWidth = 120;
            WriteLine("Welcome to C# console tetris by destbg.\n" +
                "While in-game you can access the menu by pressing the 'escape' button.\n" +
                "To start the game type the difficulty you want to begin with.\n" +
                "Types of difficulty: easy, normal, average, hard.\n");
            difficulty = 1000;
            int levelOfDifficulty = 1;
            level = 1;
            string input;
            while (true) {
                Write("difficulty: ");
                input = ReadLine().Trim().ToLower();
                if (input == "easy") break;
                else if (input == "normal") {
                    difficulty = 750;
                    levelOfDifficulty = 2;
                    break;
                }
                else if (input == "average") {
                    difficulty = 500;
                    levelOfDifficulty = 3;
                    break;
                }
                else if (input == "hard") {
                    difficulty = 300;
                    levelOfDifficulty = 4;
                    break;
                }
                else WriteLine("Wrong difficulty input, try again.\n");
            }
            CursorVisible = false;
            Clear();
            board = new Board(levelOfDifficulty);
            allowed = true;
            canMove = true;
            WindowWidth = 34;
            WindowHeight = 22;
            Write(board);
            timer = new Timer() {
                Interval = difficulty,
                AutoReset = true,
                Enabled = true
            };
            move = new Timer() {
                Interval = 150,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += TimerTick;
            move.Elapsed += Move_Elapsed;
            #endregion
            while (gameState) {
                var consoleKey = ReadKey(true).Key;
                if (!gameState) break;
                allowed = false;
                switch (consoleKey) {
                    case W: {
                        if (canMove) {
                            board.RotateBlock();
                            Write(board);
                            canMove = false;
                        }
                        break;
                    }
                    case A: {
                        if (canMove) {
                            board.MoveBlock(2);
                            Write(board);
                            canMove = false;
                        }
                        break;
                    }
                    case S: {
                        board.MoveBlock(0);
                        Write(board);
                        break;
                    }
                    case D: {
                        if (canMove) {
                            board.MoveBlock(1);
                            Write(board);
                            canMove = false;
                        }
                        break;
                    }
                    case Spacebar: {
                        if (canMove) {
                            board.InstantlyPlaceBlock();
                            Write(board);
                            canMove = false;
                        }
                        break;
                    }
                    case Escape: {
                        Menu();
                        Write(board);
                        break;
                    }
                }
                allowed = true;
            }
            while (ReadKey(true).Key != R) { }
        }

        private void Move_Elapsed(object sender, ElapsedEventArgs e) =>
            canMove = true;

        private void TimerTick(object sender, ElapsedEventArgs e) {
            if (!gameState && allowed) {
                allowed = false;
                WindowHeight = 30;
                WindowWidth = 120;
                SetCursorPosition(0, 22);
                WriteLine("\nGame ended.\nPress 'R' to restart the game.");
            }
            else if (allowed) {
                board.MoveBlock(0);
                Write(board);
            }
            int getLevel = board.Level;
            if (level != getLevel) {
                level = getLevel;
                timer = new Timer() {
                    Interval = difficulty + difficulty * getLevel * 0.1,
                    AutoReset = true,
                    Enabled = true
                };
            }
        }

        private void Menu() {
            ResetColor();
            WindowHeight = 30;
            WindowWidth = 120;
            while (true) {
                Clear();
                WriteLine("    Paused\n" +
                    "Press '1' to resume\n" +
                    "Press '2' for 'how to play'\n" +
                    "Press '3' to quit");
                var input = ReadKey(true).Key;
                if (input == D1) break;
                else if (input == D2) {
                    Clear();
                    WriteLine("1: Move the blocks left and right by presssing 'a' and 'd'\n\n" +
                        "2: Rotate the block by pressing 'w'\n\n" +
                        "3: Move the blocks faster by pressing 's'\n\n" +
                        "4: Move the blocks instantly by pressing 'space'");
                    ReadKey(true);
                }
                else if (input == D3) {
                    GameState = false;
                    break;
                }
            }
            Clear();
            WindowWidth = 34;
            WindowHeight = 22;
            board.Borders();
        }
    }
}