namespace Tetris {
    class StartUp {
        public static bool GameState = true;
        static void Main() {
            System.Console.Title = "Tetris";
            while (true) {
                while (GameState) new Game();
                GameState = true;
            }
        }
    }
}