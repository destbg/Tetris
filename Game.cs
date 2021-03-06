﻿using System.Timers;
using static System.Console;
using static System.ConsoleKey;

namespace Tetris {
    internal class Game {
        Board board;
        Timer timer, move;
        bool allowed, canMove;
        int level, difficulty;

        private static bool gameState;
        public static void EndGame() =>
            gameState = false;

        public Game() {
            StartScreen();
            var consoleKey = Backspace;
            while (gameState) {
                allowed = false;
                switch (consoleKey) {
                    case W: CaseW(); break;
                    case A: CaseA(); break;
                    case S: CaseS(); break;
                    case D: CaseD(); break;
                    case Spacebar: CaseSpacebar(); break;
                    case Escape: Menu(); break;
                }
                allowed = true;
                consoleKey = ReadKey(true).Key;
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
            var getLevel = board.Level;
            if (level != getLevel) {
                level = getLevel;
                timer = new Timer {
                    Interval = difficulty + difficulty * getLevel * 0.1,
                    AutoReset = true,
                    Enabled = true
                };
            }
        }

        private void StartScreen() {
            Clear();
            CursorVisible = true;
            gameState = true;
            WindowHeight = 30;
            WindowWidth = 120;
            WriteLine("Welcome to C# console Tetris by destbg.\n" +
                "While in-game you can access the menu by pressing the 'escape' button.\n" +
                "To start the game type the difficulty you want to begin with.\n" +
                "Types of difficulty: easy, normal, average, hard.\n");
            difficulty = 1000;
            var levelOfDifficulty = 1;
            level = 1;
            string input;
            while (true) {
                Write("difficulty: ");
                input = ReadLine().Trim().ToLower();
                if (input == "easy") break;
                if (input == "normal") {
                    difficulty = 750;
                    levelOfDifficulty = 2;
                    break;
                }
                if (input == "average") {
                    difficulty = 500;
                    levelOfDifficulty = 3;
                    break;
                }
                if (input == "hard") {
                    difficulty = 300;
                    levelOfDifficulty = 4;
                    break;
                }
                WriteLine("Wrong difficulty input, try again.\n");
            }
            CursorVisible = false;
            Clear();
            board = new Board(levelOfDifficulty);
            allowed = true;
            canMove = true;
            WindowWidth = 34;
            WindowHeight = 22;
            Write(board);
            timer = new Timer {
                Interval = difficulty,
                AutoReset = true,
                Enabled = true
            };
            move = new Timer {
                Interval = 150,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += TimerTick;
            move.Elapsed += Move_Elapsed;
        }

        private void CaseW() {
            if (canMove) {
                board.RotateBlock();
                Write(board);
                canMove = false;
            }
        }

        private void CaseA() {
            if (canMove) {
                board.MoveBlock(2);
                Write(board);
                canMove = false;
            }
        }

        private void CaseS() {
            board.MoveBlock(0);
            Write(board);
        }

        private void CaseD() {
            if (canMove) {
                board.MoveBlock(1);
                Write(board);
                canMove = false;
            }
        }

        private void CaseSpacebar() {
            if (canMove) {
                board.InstantlyPlaceBlock();
                Write(board);
                canMove = false;
            }
        }

        private void Menu() {
            ResetColor();
            WindowHeight = 30;
            WindowWidth = 120;
            Clear();
            WriteLine("    Paused\n" +
                "Press '1' to resume\n" +
                "Press '2' for 'how to play'\n" +
                "Press '3' to quit");
            System.ConsoleKey input;
            while ((input = ReadKey(true).Key) != D1)
                if (input == D2) {
                    Clear();
                    WriteLine("1: Move the blocks left and right by pressing 'a' and 'd'\n\n" +
                        "2: Rotate the block by pressing 'w'\n\n" +
                        "3: Move the blocks faster by pressing 's'\n\n" +
                        "4: Move the blocks instantly by pressing 'space'");
                    ReadKey();
                    Clear();
                    WriteLine("    Paused\n" +
                        "Press '1' to resume\n" +
                        "Press '2' for 'how to play'\n" +
                        "Press '3' to quit");
                }
                else if (input == D3) {
                    gameState = false;
                    break;
                }
            Clear();
            WindowWidth = 34;
            WindowHeight = 22;
            board.Borders();
            Write(board);
        }
    }
}