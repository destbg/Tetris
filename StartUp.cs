namespace Tetris {
    class StartUp {
        static void Main() {
            System.Console.Title = "Tetris";
            while (true) new Game();
        }
    }
}