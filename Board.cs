namespace Tetris {
    class Board {
        private int height = 21;
        private int width = 10;
        private bool[,] board;
        private Blocks block;
        private byte[] cB;//current block
        private byte[] nB;//next block
        private int[] m;//movement
        private int points;
        private bool[,] preview;

        public Board() {
            board = new bool[height, width];
            preview = new bool[4, 5];
            block = new Blocks();
            for (int h = height - 1; h < height; h++)
                for (int w = 0; w < width; w++)
                    board[h, w] = true;
            for (int h = 0; h < height; h++) {
                board[h, 0] = true;
                board[h, width - 1] = true;
            }
            nB = block.GetBlock();
            cB = block.GetBlock();
            PlaceBlock();
            points = 0;
        }

        public override string ToString() {
            string toSay = string.Empty;
            for (int i = 0; i < width; i++)
                toSay += "* ";
            toSay += " Next block:\n";
            for (int h = 0; h < height - 1; h++) {
                toSay += "* ";
                for (int w = 1; w < width - 1; w++)
                    toSay += !board[h, w] ? "  " : "■ ";
                toSay += '*';
                if (h > 0 && h < 5)
                    for (int i = 0; i < 5; i++)
                        toSay += !preview[h - 1, i] ? "  " : "■ ";
                toSay += '\n';
            }
            for (int i = 0; i < width; i++)
                toSay += "* ";
            return toSay;
        }

        private void PlaceBlock() {
            if (board[cB[0], cB[1] + 1] ||
            board[cB[2], cB[3] + 1] ||
            board[cB[4], cB[5] + 1] ||
            board[cB[6], cB[7] + 1]) {
                StartUp.GameState = false;
                return;
            }
            cB = nB;
            nB = block.GetBlock();
            board[cB[0], cB[1] + 1] = true;
            board[cB[2], cB[3] + 1] = true;
            board[cB[4], cB[5] + 1] = true;
            board[cB[6], cB[7] + 1] = true;
            preview[cB[0], cB[1]] = false;
            preview[cB[2], cB[3]] = false;
            preview[cB[4], cB[5]] = false;
            preview[cB[6], cB[7]] = false;
            preview[nB[0], nB[1]] = true;
            preview[nB[2], nB[3]] = true;
            preview[nB[4], nB[5]] = true;
            preview[nB[6], nB[7]] = true;
            m = new int[] { 0, 1 };
        }

        public void MoveBlock(int to) {
            SetFalse();
            if (to == 0)
                if (IsInsideH(m[0] + 1) || IsInsideH(m[0] + 1)
                    || IsInsideH(m[0] + 1) || IsInsideH(m[0] + 1)) { }
                else m[0]++;
            else if (to == 1)
                if (IsInsideW(m[1] + 1) || IsInsideW(m[1] + 1)
                || IsInsideW(m[1] + 1) || IsInsideW(m[1] + 1)) { }
                else m[1]++;
            else if (to == 2)
                if (IsInsideW(m[1] - 1) || IsInsideW(m[1] - 1)
                || IsInsideW(m[1] - 1) || IsInsideW(m[1] - 1)) { }
                else m[1]--;
            SetTrue();
        }

        public void RotateBlock() {
            if (cB[8] == 2) return;
            SetFalse();
            byte[] arr;
            if (cB[8] == 1) arr = block.Rotate1(cB[9]);
            else if (cB[8] == 3) arr = block.Rotate3(cB[9]);
            else if (cB[8] == 4) arr = block.Rotate4(cB[9]);
            else if (cB[8] == 5) arr = block.Rotate5(cB[9]);
            else if (cB[8] == 6) arr = block.Rotate6(cB[9]);
            else arr = block.Rotate7(cB[9]);
            if (!IsInside(arr[0] + m[0], arr[1] + m[1]) ||
                !IsInside(arr[2] + m[0], arr[3] + m[1]) ||
                !IsInside(arr[4] + m[0], arr[5] + m[1]) ||
                !IsInside(arr[6] + m[0], arr[7] + m[1])) { }
            else if (board[arr[0] + m[0], arr[1] + m[1]] ||
                board[arr[2] + m[0], arr[3] + m[1]] ||
                board[arr[4] + m[0], arr[5] + m[1]] ||
                board[arr[6] + m[0], arr[7] + m[1]]) { }
            else cB = arr;
            SetTrue();
        }

        private void CheckRows() {
            int linesCleared = 0;
            for (int h = height - 2; h > 0; h--) {
                bool full = true;
                for (int w = 1; w < width - 1; w++)
                    if (!board[h, w]) { full = false; break; }
                if (full) {
                    for (int w = 1; w < width - 1; w++)
                        board[h, w] = false;
                    for (int i = h; i > 0; i--)
                        for (int w = 1; w < width - 1; w++)
                            if (board[i, w]) {
                                board[i, w] = false;
                                board[i + 1, w] = true;
                            }
                    h++;
                    linesCleared++;
                }
            }
            points += linesCleared == 0 ? 0 : linesCleared == 1 ? 40
                : linesCleared == 2 ? 100 : linesCleared == 3 ? 300 : 1200;
        }

        public int GetScore() =>
            points;

        private void SetFalse() {
            board[cB[0] + m[0], cB[1] + m[1]] = false;
            board[cB[2] + m[0], cB[3] + m[1]] = false;
            board[cB[4] + m[0], cB[5] + m[1]] = false;
            board[cB[6] + m[0], cB[7] + m[1]] = false;
        }

        private void SetTrue() {
            board[cB[0] + m[0], cB[1] + m[1]] = true;
            board[cB[2] + m[0], cB[3] + m[1]] = true;
            board[cB[4] + m[0], cB[5] + m[1]] = true;
            board[cB[6] + m[0], cB[7] + m[1]] = true;
        }

        private bool IsInsideH(int h) {
            if (board[cB[0] + h, cB[1] + m[1]]
                || board[cB[2] + h, cB[3] + m[1]]
                || board[cB[4] + h, cB[5] + m[1]]
                || board[cB[6] + h, cB[7] + m[1]]) {
                SetTrue();
                CheckRows();
                PlaceBlock();
                return true;
            }
            return false;
        }
        private bool IsInsideW(int w) {
            if (board[cB[0] + m[0], cB[1] + w] ||
                board[cB[2] + m[0], cB[3] + w] ||
                board[cB[4] + m[0], cB[5] + w] ||
                board[cB[6] + m[0], cB[7] + w]) return true;
            return false;
        }

        private bool IsInside(int h, int w) =>
            h >= 1 && w >= 0 && h <= height && w <= width;
    }
}