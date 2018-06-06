namespace Tetris {
    internal class StartUp {
        private static void Main() {
            System.Console.Title = "Tetris";
            while (true) new Game();
        }
    }
}