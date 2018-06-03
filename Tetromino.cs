using static System.ConsoleColor;

namespace Tetris {
    class Tetromino {
        public bool B { get; }
        public System.ConsoleColor Color { get; }

        public Tetromino(bool block = false, int color = 0) { 
            B = block;
            switch (color) {
                case 1:
                    Color = Cyan;
                    break;
                case 2:
                    Color = Yellow;
                    break;
                case 3:
                    Color = DarkMagenta;
                    break;
                case 4:
                    Color = Red;
                    break;
                case 5:
                    Color = DarkGreen;
                    break;
                case 6:
                    Color = DarkCyan;
                    break;
                case 7:
                    Color = Blue;
                    break;
                default:
                    Color = DarkGray;
                    break;
            }
        }

        public override string ToString() {
            System.Console.ForegroundColor = Color;
            return !B ? "  " : "■ ";
        }
    }
}