using static System.ConsoleColor;

namespace Tetris {
    class Tetromino {
        public bool B { get; }
        public System.ConsoleColor Color { get; }

        public Tetromino(bool block = false, byte color = 9) {
            B = block;
            if (color == 1) Color = Cyan;
            else if (color == 2) Color = Yellow;
            else if (color == 3) Color = DarkMagenta;
            else if (color == 4) Color = Red;
            else if (color == 5) Color = DarkGreen;
            else if (color == 6) Color = DarkCyan;
            else if (color == 7) Color = Blue;
            else Color = DarkGray;
        }

        public override string ToString() =>
            !B ? "  " : "■ ";
    }
}