namespace Tetris {
    class Board {
        private readonly int height, width;
        private int level, linesCleared;
        private long points;
        private readonly bool[,] board, preview;
        private Blocks block;
        private byte[] cB, nB;
        private int[] m;

        public int CurrentLevel {
            get => level;
        }

        public Board() {
            height = 21;
            width = 10;
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
            linesCleared = 0;
            level = 1;
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
                else if (h > 5 && h < 11 && h != 7 && h != 9)
                    toSay += h == 6 ? " Level " + level
                        : h == 8 ? " Cleared " + linesCleared
                        : " Points " + points;
                toSay += '\n';
            }
            for (int i = 0; i < width; i++)
                toSay += "* ";
            return toSay;
        }

        public void InstantlyPlaceBlock() {
            byte check = nB[8];
            SetFalse();
            byte loop = 0;
            while (check != cB[8] && loop < 20) {
                if (board[cB[0] + m[0] + 1, cB[1] + m[1]]
                || board[cB[2] + m[0] + 1, cB[3] + m[1]]
                || board[cB[4] + m[0] + 1, cB[5] + m[1]]
                || board[cB[6] + m[0] + 1, cB[7] + m[1]]) {
                    SetTrue();
                    CheckRows();
                    PlaceBlock();
                }
                else m[0]++;
                loop++;
            }
            SetTrue();
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
            while (cB[8] == nB[8])
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
                if (board[cB[0] + m[0] + 1, cB[1] + m[1]]
                || board[cB[2] + m[0] + 1, cB[3] + m[1]]
                || board[cB[4] + m[0] + 1, cB[5] + m[1]]
                || board[cB[6] + m[0] + 1, cB[7] + m[1]]) {
                    SetTrue();
                    CheckRows();
                    PlaceBlock();
                }
                else m[0]++;
            else if (to == 1)
                if (board[cB[0] + m[0], cB[1] + m[1] + 1] ||
                board[cB[2] + m[0], cB[3] + m[1] + 1] ||
                board[cB[4] + m[0], cB[5] + m[1] + 1] ||
                board[cB[6] + m[0], cB[7] + m[1] + 1]) { }
                else m[1]++;
            else if (to == 2)
                if (board[cB[0] + m[0], cB[1] + m[1] - 1] ||
                board[cB[2] + m[0], cB[3] + m[1] - 1] ||
                board[cB[4] + m[0], cB[5] + m[1] - 1] ||
                board[cB[6] + m[0], cB[7] + m[1] - 1]) { }
                else m[1]--;
            SetTrue();
        }

        public void RotateBlock() {
            if (cB[8] == 2) return;
            SetFalse();
            byte[] arr = block.Rotate(cB[9], cB[8]);
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
            int linesC = 0;
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
                    linesC++;
                }
            }
            if (linesC > 0) {
                points += linesC == 1 ? 40 * level
                    : linesC == 2 ? 100 * level : linesC == 3 ? 300 : 1200 * level;
                linesCleared += linesC;
                if (linesCleared < 5) level = 1;
                else if (linesCleared < 10) level = 2;
                else if (linesCleared < 15) level = 3;
                else if (linesCleared < 25) level = 4;
                else if (linesCleared < 35) level = 5;
                else if (linesCleared < 50) level = 6;
                else if (linesCleared < 70) level = 7;
                else if (linesCleared < 90) level = 8;
                else if (linesCleared < 110) level = 9;
                else if (linesCleared < 150) level = 10;
            }
        }

        public long GetScore() =>
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

        private bool IsInside(int h, int w) =>
            h >= 1 && w >= 0 && h <= height && w <= width;
    }
}