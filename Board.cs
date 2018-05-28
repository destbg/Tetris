using static System.Console;

namespace Tetris {
    class Board {
        private readonly int height, width, lOD;
        private readonly string lODs;
        private int linesCleared;
        private long points;
        private readonly Tetromino[,] board, preview;
        private Blocks block;
        private byte[] cB, nB;
        private readonly byte[] prv;
        private int[] m;

        public int Level { get; private set; }

        public Board(int lOD) {
            height = 21;
            width = 10;
            board = new Tetromino[height, width];
            preview = new Tetromino[4, 5];
            block = new Blocks();
            for (int h = 0; h < height; h++) {
                for (int w = 1; w < width - 1; w++)
                    board[h, w] = new Tetromino();
                board[h, 0] = new Tetromino(true);
                board[h, width - 1] = new Tetromino(true);
                if (h == height - 1)
                    for (int w = 0; w < width; w++)
                        board[h, w] = new Tetromino(true);
            }
            for (int h = 0; h < 4; h++)
                for (int w = 0; w < 5; w++)
                    preview[h, w] = new Tetromino();
            nB = block.GetBlock();
            cB = block.GetBlock();
            prv = new byte[4];
            PlaceBlock();
            points = 0;
            linesCleared = 0;
            Level = 1;
            this.lOD = lOD;
            lODs = (lOD == 1 ? "easy" : lOD == 2 ? "normal" : lOD == 3 ? "average" : "hard");
            Borders();
        }

        public override string ToString() {
            for (int h = 0; h < height - 1; h++) {
                SetCursorPosition(2, h + 1);
                for (int w = 1; w < width - 1; w++) {
                    ForegroundColor = board[h, w].Color;
                    Write(!board[h, w].B ? "  " : "■ ");
                }
                WriteLine();
            }
            ResetColor();
            return string.Empty;
        }

        public void Borders() {
            ResetColor();
            for (int i = 0; i < width; i++) {
                SetCursorPosition(i * 2, 0);
                Write('*');
            }
            SetCursorPosition(0, 1);
            for (int h = 1; h < height; h++) {
                Write('*');
                SetCursorPosition(18, h);
                WriteLine('*');
            }
            for (int i = 0; i < width; i++) {
                SetCursorPosition(i * 2, 21);
                Write('*');
            }
            SetCursorPosition(20, 0);
            WriteLine("Next block:");
            SetCursorPosition(20, 7);
            Write("Level " + Level);
            SetCursorPosition(20, 9);
            Write("Cleared " + linesCleared);
            SetCursorPosition(20, 11);
            Write("Points " + points);
            SetCursorPosition(20, 13);
            Write("L.O.D " + lODs);
        }

        public void InstantlyPlaceBlock() {
            SetFalse();
            PreviewFalse();
            byte b = cB[8];
            board[prv[0], cB[1] + m[1]] = new Tetromino(true, b);
            board[prv[1], cB[3] + m[1]] = new Tetromino(true, b);
            board[prv[2], cB[5] + m[1]] = new Tetromino(true, b);
            board[prv[3], cB[7] + m[1]] = new Tetromino(true, b);
            CheckRows();
            PlaceBlock();
            ResetColor();
            SetCursorPosition(0, 21);
            Write('*');
        }

        private void PlaceBlock() {
            if (board[cB[0], cB[1] + 1].B ||
            board[cB[2], cB[3] + 1].B ||
            board[cB[4], cB[5] + 1].B ||
            board[cB[6], cB[7] + 1].B) {
                Game.GameState = false;
                return;
            }
            cB = nB;
            while (cB[8] == nB[8])
                nB = block.GetBlock();
            byte b = cB[8];
            board[cB[0], cB[1] + 1] = new Tetromino(true, b);
            board[cB[2], cB[3] + 1] = new Tetromino(true, b);
            board[cB[4], cB[5] + 1] = new Tetromino(true, b);
            board[cB[6], cB[7] + 1] = new Tetromino(true, b);
            preview[cB[0], cB[1]] = new Tetromino();
            preview[cB[2], cB[3]] = new Tetromino();
            preview[cB[4], cB[5]] = new Tetromino();
            preview[cB[6], cB[7]] = new Tetromino();
            b = nB[8];
            preview[nB[0], nB[1]] = new Tetromino(true, b);
            preview[nB[2], nB[3]] = new Tetromino(true, b);
            preview[nB[4], nB[5]] = new Tetromino(true, b);
            preview[nB[6], nB[7]] = new Tetromino(true, b);
            m = new int[] { 0, 1 };
            for (int h = 0; h < 4; h++)
                for (int w = 1; w < 5; w++) {
                    SetCursorPosition(w * 2 + 19, h + 2);
                    ForegroundColor = preview[h, w].Color;
                    Write(!preview[h, w].B ? ' ' : '■');
                }
            SetFalse();
            PreviewTrue();
            SetTrue();
        }

        public void MoveBlock(int to) {
            SetFalse();
            PreviewFalse();
            if (to == 0)
                if (board[cB[0] + m[0] + 1, cB[1] + m[1]].B
                || board[cB[2] + m[0] + 1, cB[3] + m[1]].B
                || board[cB[4] + m[0] + 1, cB[5] + m[1]].B
                || board[cB[6] + m[0] + 1, cB[7] + m[1]].B) {
                    SetTrue();
                    CheckRows();
                    PlaceBlock();
                    return;
                }
                else m[0]++;
            else if (to == 1)
                if (board[cB[0] + m[0], cB[1] + m[1] + 1].B ||
                board[cB[2] + m[0], cB[3] + m[1] + 1].B ||
                board[cB[4] + m[0], cB[5] + m[1] + 1].B ||
                board[cB[6] + m[0], cB[7] + m[1] + 1].B) { }
                else m[1]++;
            else if (to == 2)
                if (board[cB[0] + m[0], cB[1] + m[1] - 1].B ||
                board[cB[2] + m[0], cB[3] + m[1] - 1].B ||
                board[cB[4] + m[0], cB[5] + m[1] - 1].B ||
                board[cB[6] + m[0], cB[7] + m[1] - 1].B) { }
                else m[1]--;
            PreviewTrue();
            SetTrue();
        }

        public void RotateBlock() {
            if (cB[8] == 2) return;
            SetFalse();
            PreviewFalse();
            byte[] arr = block.Rotate(cB[9], cB[8]);
            if (!IsInside(arr[0] + m[0], arr[1] + m[1]) ||
                !IsInside(arr[2] + m[0], arr[3] + m[1]) ||
                !IsInside(arr[4] + m[0], arr[5] + m[1]) ||
                !IsInside(arr[6] + m[0], arr[7] + m[1])) { }
            else if (board[arr[0] + m[0], arr[1] + m[1]].B ||
                board[arr[2] + m[0], arr[3] + m[1]].B ||
                board[arr[4] + m[0], arr[5] + m[1]].B ||
                board[arr[6] + m[0], arr[7] + m[1]].B) { }
            else cB = arr;
            PreviewTrue();
            SetTrue();
        }

        private void CheckRows() {
            byte combo = 0;
            for (int h = 0; h < height - 1; h++) {
                bool full = true;
                for (int w = 1; w < width - 1; w++)
                    if (!board[h, w].B) { full = false; break; }
                if (full) {
                    for (int w = 1; w < width - 1; w++)
                        board[h, w] = new Tetromino();
                    for (int i = h; i > 0; i--)
                        for (int w = 1; w < width - 1; w++)
                            if (board[i, w].B) {
                                var t = board[i, w];
                                board[i, w] = new Tetromino();
                                board[i + 1, w] = t;
                            }
                    combo++;
                }
            }
            if (combo > 0) {
                points += combo == 1 ? 40 * Level * lOD : combo == 2 ? 100 * Level * lOD
                    : combo == 3 ? 300 : 1200 * Level * lOD;
                linesCleared += combo;
                Level = linesCleared / 15 + 1;
                Borders();
            }
        }

        private void PreviewFalse() {
            board[prv[0], cB[1] + m[1]] = new Tetromino();
            board[prv[1], cB[3] + m[1]] = new Tetromino();
            board[prv[2], cB[5] + m[1]] = new Tetromino();
            board[prv[3], cB[7] + m[1]] = new Tetromino();
        }

        private void PreviewTrue() {
            byte pos = 0;
            while (true) {
                if (board[cB[0] + pos + 1, cB[1] + m[1]].B
                || board[cB[2] + pos + 1, cB[3] + m[1]].B
                || board[cB[4] + pos + 1, cB[5] + m[1]].B
                || board[cB[6] + pos + 1, cB[7] + m[1]].B) {
                    for (int i = 0; i < 4; i++)
                        prv[i] = (byte)(cB[i * 2] + pos);
                    board[prv[0], cB[1] + m[1]] = new Tetromino(true);
                    board[prv[1], cB[3] + m[1]] = new Tetromino(true);
                    board[prv[2], cB[5] + m[1]] = new Tetromino(true);
                    board[prv[3], cB[7] + m[1]] = new Tetromino(true);
                    return;
                }
                else pos++;
            }
        }

        private void SetFalse() {
            board[cB[0] + m[0], cB[1] + m[1]] = new Tetromino();
            board[cB[2] + m[0], cB[3] + m[1]] = new Tetromino();
            board[cB[4] + m[0], cB[5] + m[1]] = new Tetromino();
            board[cB[6] + m[0], cB[7] + m[1]] = new Tetromino();
        }

        private void SetTrue() {
            byte b = cB[8];
            board[cB[0] + m[0], cB[1] + m[1]] = new Tetromino(true, b);
            board[cB[2] + m[0], cB[3] + m[1]] = new Tetromino(true, b);
            board[cB[4] + m[0], cB[5] + m[1]] = new Tetromino(true, b);
            board[cB[6] + m[0], cB[7] + m[1]] = new Tetromino(true, b);
        }

        private bool IsInside(int h, int w) =>
            h >= 1 && w >= 0 && h <= height && w <= width;
    }
}