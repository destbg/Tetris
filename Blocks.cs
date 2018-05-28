namespace Tetris {
    class Blocks {
        private readonly byte[][] block;
        private System.Random rnd;
        private readonly byte[] recentBlocks;
        private byte pos;

        public Blocks() {
            block = new byte[][] {
                    new byte[] { 0, 3, 1, 3, 2, 3, 3, 3, 1, 1 },
                    new byte[] { 0, 3, 0, 4, 1, 3, 1, 4, 2, 1 },
                    new byte[] { 0, 3, 1, 2, 1, 3, 1, 4, 3, 1 },
                    new byte[] { 0, 4, 1, 4, 1, 3, 2, 3, 4, 1 },
                    new byte[] { 0, 3, 1, 3, 1, 4, 2, 4, 5, 1 },
                    new byte[] { 0, 3, 1, 3, 2, 3, 2, 4, 6, 1 },
                    new byte[] { 0, 4, 1, 4, 2, 4, 2, 3, 7, 1 } };
            rnd = new System.Random();
            recentBlocks = new byte[3];
            pos = 2;
        }

        public byte[] GetBlock() {
            while (true) {
                byte[] temp = block[rnd.Next(7)];
                bool contains = false;
                for (int i = 0; i < 3; i++)
                    if (recentBlocks[i] == temp[8]) { contains = true; break; }
                if (!contains) {
                    pos++;
                    if (pos == 3) pos = 0;
                    recentBlocks[pos] = temp[8];
                    return temp;
                }
            }
        }

        public byte[] Rotate(byte r, byte b) =>
            b == 1 ? (r == 1 ? new byte[] { 1, 5, 1, 4, 1, 3, 1, 2, 1, 2 }
            : new byte[] { 0, 3, 1, 3, 2, 3, 3, 3, 1, 1 })
            : b == 3 ? (r == 1 ? new byte[] { 1, 4, 0, 3, 1, 3, 2, 3, 3, 2 }
            : r == 2 ? new byte[] { 2, 3, 1, 2, 1, 3, 1, 4, 3, 3 }
            : r == 3 ? new byte[] { 1, 2, 2, 3, 1, 3, 0, 3, 3, 4 }
            : new byte[] { 0, 3, 1, 2, 1, 3, 1, 4, 3, 1 })
            : b == 4 ? (r == 1 ? new byte[] { 2, 5, 2, 4, 1, 4, 1, 3, 4, 2 }
            : new byte[] { 0, 4, 1, 4, 1, 3, 2, 3, 4, 1 })
            : b == 5 ? (r == 1 ? new byte[] { 1, 4, 1, 3, 2, 3, 2, 2, 5, 2 }
            : new byte[] { 0, 3, 1, 3, 1, 4, 2, 4, 5, 1 })
            : b == 6 ? (r == 1 ? new byte[] { 1, 4, 1, 3, 1, 2, 2, 2, 6, 2 }
            : r == 2 ? new byte[] { 2, 3, 1, 3, 0, 3, 0, 2, 6, 3 }
            : r == 3 ? new byte[] { 1, 2, 1, 3, 1, 4, 0, 4, 6, 4 }
            : new byte[] { 0, 3, 1, 3, 2, 3, 2, 4, 6, 1 })
            : (r == 1 ? new byte[] { 1, 5, 1, 4, 1, 3, 0, 3, 7, 2 }
            : r == 2 ? new byte[] { 2, 3, 1, 3, 0, 3, 0, 4, 7, 3 }
            : r == 3 ? new byte[] { 1, 2, 1, 3, 1, 4, 2, 4, 7, 4 }
            : new byte[] { 0, 4, 1, 4, 2, 4, 2, 3, 7, 1 });
    }
}