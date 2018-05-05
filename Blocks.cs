namespace Tetris {
    class Blocks {
        private byte[][] block;
        private System.Random rnd;

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
        }

        public byte[] GetBlock() =>
            block[rnd.Next(7)];

        public byte[] Rotate1(byte r) =>
            r == 1 ? new byte[] { 1, 5, 1, 4, 1, 3, 1, 2, 1, 2 }
            : new byte[] { 0, 3, 1, 3, 2, 3, 3, 3, 1, 1 };

        public byte[] Rotate3(byte r) =>
            r == 1 ? new byte[] { 1, 4, 0, 3, 1, 3, 2, 3, 3, 2 }
            : r == 2 ? new byte[] { 2, 3, 1, 2, 1, 3, 1, 4, 3, 3 }
            : r == 3 ? new byte[] { 1, 2, 2, 3, 1, 3, 0, 3, 3, 4 }
            : new byte[] { 0, 3, 1, 2, 1, 3, 1, 4, 3, 1 };

        public byte[] Rotate4(byte r) =>
            r == 1 ? new byte[] { 2, 5, 2, 4, 1, 4, 1, 3, 4, 2 }
            : new byte[] { 0, 4, 1, 4, 1, 3, 2, 3, 4, 1 };

        public byte[] Rotate5(byte r) =>
            r == 1 ? new byte[] { 1, 4, 1, 3, 2, 3, 2, 2, 5, 2 }
            : new byte[] { 0, 3, 1, 3, 1, 4, 2, 4, 5, 1 };

        public byte[] Rotate6(byte r) =>
            r == 1 ? new byte[] { 1, 4, 1, 3, 1, 2, 2, 2, 6, 2 }
            : r == 2 ? new byte[] { 2, 3, 1, 3, 0, 3, 0, 2, 6, 3 }
            : r == 3 ? new byte[] { 1, 2, 1, 3, 1, 4, 0, 4, 6, 4 }
            : new byte[] { 0, 3, 1, 3, 2, 3, 2, 4, 6, 1 };

        public byte[] Rotate7(byte r) =>
            r == 1 ? new byte[] { 1, 5, 1, 4, 1, 3, 0, 3, 7, 2 }
            : r == 2 ? new byte[] { 2, 3, 1, 3, 0, 3, 0, 4, 7, 3 }
            : r == 3 ? new byte[] { 1, 2, 1, 3, 1, 4, 2, 4, 7, 4 }
            : new byte[] { 0, 4, 1, 4, 2, 4, 2, 3, 7, 1 };
    }
}
