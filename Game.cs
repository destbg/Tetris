using System.Timers;
using static System.Console;

namespace Tetris {
    class Game {
        private Board board;
        private Timer timer;
        private Timer delay;
        private bool onDelay;
        private bool allowed;

        public Game() {
            StartScreen();
            while (StartUp.GameState) {
                char input = ReadKey(true).KeyChar;
                switch (input) {
                    case 'w': {
                        if (!onDelay) {
                            board.RotateBlock();
                            SetCursorPosition(0, 0);
                            WriteLine(board.ToString());
                            onDelay = true;
                        }
                        break;
                    }
                    case 'a': {
                        board.MoveBlock(2);
                        SetCursorPosition(0, 0);
                        WriteLine(board.ToString());
                        break;
                    }
                    case 's': {
                        TimerTick();
                        break;
                    }
                    case 'd': {
                        board.MoveBlock(1);
                        SetCursorPosition(0, 0);
                        WriteLine(board.ToString());
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
            while ((read = ReadLine().ToLower()) != "end") { }
        }

        private void StartScreen() {
            Clear();
            WindowHeight = 30;
            WindowWidth = 120;
            board = new Board();
            WriteLine("Welcome to tetris, press anything to start.");
            ReadKey();
            Clear();
            onDelay = false;
            allowed = true;
            WindowWidth = 33;
            WindowHeight = 25;
            TimerTick();
            timer = new Timer() {
                Interval = 1000,
                AutoReset = true,
                Enabled = true
            };
            delay = new Timer() {
                Interval = 200,
                AutoReset = true,
                Enabled = true
            };
            timer.Elapsed += TimerTick;
            delay.Elapsed += Delay_Elapsed;
        }

        private void Delay_Elapsed(object sender, ElapsedEventArgs e) {
            onDelay = false;
        }

        private void TimerTick(object sender = null, ElapsedEventArgs e = null) {
            if (allowed) {
                board.MoveBlock(0);
                SetCursorPosition(0, 0);
                WriteLine(board.ToString());
            }
        }
    }
}
