using System.Linq;
using static System.Console;

namespace Tetris {
    internal class Board {
        readonly Tetromino[][] board, preview;
        readonly int height, width, lOD;
        readonly string lODs;
        readonly int[] prv;
        int[] cB, nB;
        int linesCleared, move;
        long points;
        readonly Blocks block;

        public int Level { get; private set; }

        public Board(int lOD) {
            height = 24;
            width = 10;
            board = new Tetromino[height][];
            preview = new Tetromino[4][];
            block = new Blocks();
            for (int h = 0; h < height; h++) {
                board[h] = new Tetromino[width];
                for (int w = 1; w < width - 1; w++)
                    board[h][w] = new Tetromino();
                board[h][0] = new Tetromino(true);
                board[h][width - 1] = new Tetromino(true);
            }
            for (int w = 0; w < width; w++)
                board[height - 1][w] = new Tetromino(true);
            for (int h = 0; h < 4; h++) {
                preview[h] = new Tetromino[6];
                for (int w = 0; w < 6; w++)
                    preview[h][w] = new Tetromino();
            }
            nB = block.GetBlock();
            cB = block.GetBlock();
            prv = new int[4];
            move = 0;
            Level = 1;
            points = 0;
            linesCleared = 0;
            this.lOD = lOD;
            lODs = (lOD == 1 ? "easy" : lOD == 2 ? "normal" : lOD == 3 ? "average" : "hard");
            PlaceBlock();
        }

        public override string ToString() {
            for (int h = 3; h < height - 1; h++) {
                SetCursorPosition(2, h - 2);
                for (int w = 1; w < width - 1; w++)
                    Write(board[h][w]);
            }
            ResetColor();
            return string.Empty;
        }

        public void Borders() {
            for (int h = 0; h < 4; h++)
                for (int w = 0; w < 6; w++) {
                    SetCursorPosition(w * 2 + 17, h + 2);
                    Write(preview[h][w]);
                }
            ResetColor();
            for (int i = 0; i < width; i++) {
                SetCursorPosition(i * 2, 0);
                Write('*');
                SetCursorPosition(i * 2, 21);
                Write('*');
            }
            for (int h = 1; h < height - 3; h++) {
                SetCursorPosition(18, h);
                Write("*\r*");
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
            cB[0] = prv[0];
            cB[2] = prv[1];
            cB[4] = prv[2];
            cB[6] = prv[3];
            SetTrue();
            CheckRows();
            PlaceBlock();
        }

        private void PlaceBlock() {
            if (EndGame()) {
                Game.EndGame();
                return;
            }
            cB = nB;
            preview[cB[0]][cB[1]] = new Tetromino();
            preview[cB[2]][cB[3]] = new Tetromino();
            preview[cB[4]][cB[5]] = new Tetromino();
            preview[cB[6]][cB[7]] = new Tetromino();
            while (cB[6] < 3) {
                cB[0]++;
                cB[2]++;
                cB[4]++;
                cB[6]++;
            }
            if (move > 3) move = 3;
            else if (move < -2 && cB[8] == 3) move = -2;
            else if (move < -3) move = -3;
            cB[1] += move;
            cB[3] += move;
            cB[5] += move;
            cB[7] += move;
            nB = block.GetBlock();
            var b = nB[8];
            preview[nB[0]][nB[1]] = new Tetromino(true, b);
            preview[nB[2]][nB[3]] = new Tetromino(true, b);
            preview[nB[4]][nB[5]] = new Tetromino(true, b);
            preview[nB[6]][nB[7]] = new Tetromino(true, b);
            PreviewTrue();
            Borders();
        }

        public void MoveBlock(int to) {
            SetFalse();
            PreviewFalse();
            switch (to) {
                case 0:
                    if (CheckBelow(1)) {
                        SetTrue();
                        CheckRows();
                        PlaceBlock();
                        return;
                    }
                    cB[0]++; cB[2]++; cB[4]++; cB[6]++;
                    break;

                case 1:
                    if (CheckSides(1)) break;
                    cB[1]++; cB[3]++; cB[5]++; cB[7]++; move++;
                    break;

                case 2:
                    if (CheckSides(-1)) break;
                    cB[1]--; cB[3]--; cB[5]--; cB[7]--; move--;
                    break;
            }
            PreviewTrue();
        }

        public void RotateBlock() {
            if (cB[8] == 2) return;
            SetFalse();
            PreviewFalse();
            var arr = block.Rotate(cB[0], cB[1], cB[2], cB[3], cB[4], cB[5], cB[6], cB[7], cB[8], cB[9]);
            if (!IsInside(arr[0], arr[1], arr[2], arr[3], arr[4], arr[5], arr[6], arr[7])) { }
            else if (board[arr[0]][arr[1]].B || board[arr[2]][arr[3]].B ||
                board[arr[4]][arr[5]].B || board[arr[6]][arr[7]].B) { }
            else cB = arr;
            PreviewTrue();
        }

        private void CheckRows() {
            var combo = 0;
            for (int h = 4; h < height - 1; h++) {
                if (!board[h].All(x => x.B)) continue;
                for (int w = 1; w < width - 1; w++)
                    board[h][w] = new Tetromino();
                for (int i = h; i > 4; i--)
                    for (int w = 1; w < width - 1; w++)
                        if (board[i][w].B) {
                            var t = board[i][w];
                            board[i][w] = new Tetromino();
                            board[i + 1][w] = t;
                        }

                combo++;
            }
            if (combo <= 0) return;
            points += combo == 1 ? 40 * Level * lOD : combo == 2 ? 100 * Level * lOD
                : combo == 3 ? 300 : 1200 * Level * lOD;
            linesCleared += combo;
            Level = linesCleared / 10 + 1;
        }

        private void PreviewFalse() {
            board[prv[0]][cB[1]] = new Tetromino();
            board[prv[1]][cB[3]] = new Tetromino();
            board[prv[2]][cB[5]] = new Tetromino();
            board[prv[3]][cB[7]] = new Tetromino();
        }

        private void PreviewTrue() {
            var pos = 0;
            while (true) {
                if (CheckBelow(pos + 1)) {
                    prv[0] = cB[0] + pos;
                    prv[1] = cB[2] + pos;
                    prv[2] = cB[4] + pos;
                    prv[3] = cB[6] + pos;
                    board[prv[0]][cB[1]] = new Tetromino(true);
                    board[prv[1]][cB[3]] = new Tetromino(true);
                    board[prv[2]][cB[5]] = new Tetromino(true);
                    board[prv[3]][cB[7]] = new Tetromino(true);
                    SetTrue();
                    return;
                }
                pos++;
            }
        }

        private void SetFalse() {
            board[cB[0]][cB[1]] = new Tetromino();
            board[cB[2]][cB[3]] = new Tetromino();
            board[cB[4]][cB[5]] = new Tetromino();
            board[cB[6]][cB[7]] = new Tetromino();
        }

        private void SetTrue() {
            var b = cB[8];
            board[cB[0]][cB[1]] = new Tetromino(true, b);
            board[cB[2]][cB[3]] = new Tetromino(true, b);
            board[cB[4]][cB[5]] = new Tetromino(true, b);
            board[cB[6]][cB[7]] = new Tetromino(true, b);
        }

        private bool IsInside(int h1, int w1, int h2, int w2, int h3, int w3, int h4, int w4) =>
            w1 > 0 && h1 < height - 1 && w1 < width - 1 ||
            w2 > 0 && h2 < height - 1 && w2 < width - 1 ||
            w3 > 0 && h3 < height - 1 && w3 < width - 1 ||
            w4 > 0 && h4 < height - 1 && w4 < width - 1;

        private bool EndGame() =>
            board[3][1].B || board[3][2].B || board[3][3].B || board[3][4].B ||
            board[3][5].B || board[3][6].B || board[3][7].B || board[3][8].B;

        private bool CheckSides(int side) =>
            board[cB[0]][cB[1] + side].B || board[cB[2]][cB[3] + side].B ||
            board[cB[4]][cB[5] + side].B || board[cB[6]][cB[7] + side].B;

        private bool CheckBelow(int value) =>
            board[cB[0] + value][cB[1]].B || board[cB[2] + value][cB[3]].B ||
            board[cB[4] + value][cB[5]].B || board[cB[6] + value][cB[7]].B;
    }
}